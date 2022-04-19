package com.pluralsight.reactive.document.service;

import com.pluralsight.reactive.document.exception.InternalFailureException;
import com.pluralsight.reactive.document.exception.ResourceNotFoundException;
import com.pluralsight.reactive.document.exception.UpstreamDependencyException;
import com.pluralsight.reactive.document.mapper.DocumentMapper;
import com.pluralsight.reactive.document.model.domain.Document;
import com.pluralsight.reactive.document.model.dto.DocumentDto;
import com.pluralsight.reactive.model.domain.*;
import lombok.extern.slf4j.Slf4j;
import org.apache.kafka.clients.producer.KafkaProducer;
import org.apache.kafka.clients.producer.ProducerRecord;
import org.apache.kafka.clients.producer.RecordMetadata;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.context.annotation.Profile;
import org.springframework.core.io.ByteArrayResource;
import org.springframework.core.io.Resource;
import org.springframework.dao.DataAccessException;
import org.springframework.dao.IncorrectResultSizeDataAccessException;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.core.Authentication;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.util.DigestUtils;
import org.springframework.util.StringUtils;

import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.time.Instant;
import java.util.ArrayList;
import java.util.Collection;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.Future;

@Service
@Slf4j
@Profile(value = "m04")
public class SimpleDocumentServiceM04 implements DocumentService {

    private final JdbcTemplate readJdbcTemplate;
    private final JdbcTemplate writeJdbcTemplate;
    private final DocumentMapper documentMapper;
    private final KafkaProducer<String, DocumentLockedEvent> documentLockedEventKafkaProducer;
    private final KafkaProducer<String, DocumentUnlockedEvent> documentUnlockedEventKafkaProducer;

    @Autowired
    public SimpleDocumentServiceM04(@Qualifier(value = "ReadJdbcTemplate") final JdbcTemplate readJdbcTemplate,
                                    @Qualifier(value = "WriteJdbcTemplate") final JdbcTemplate writeJdbcTemplate,
                                    final DocumentMapper documentMapper,
                                    final KafkaProducer<String, DocumentLockedEvent> documentLockedEventKafkaProducer,
                                    final KafkaProducer<String, DocumentUnlockedEvent> documentUnlockedEventKafkaProducer) {
        this.readJdbcTemplate = readJdbcTemplate;
        this.writeJdbcTemplate = writeJdbcTemplate;
        this.documentMapper = documentMapper;
        this.documentLockedEventKafkaProducer = documentLockedEventKafkaProducer;
        this.documentUnlockedEventKafkaProducer = documentUnlockedEventKafkaProducer;
    }

    @Override
    @Transactional
    public String uploadDocument(final Authentication authentication,
                                 final String name,
                                 final byte[] bytes) {
        try {
            final String eTag = DigestUtils.md5DigestAsHex(bytes);
            final String update =
                    "INSERT IGNORE INTO document (principal, name, e_tag, content, created_at, updated_at) VALUES (?, ?, ?, ?, ?, ?);";

            final Timestamp now = Timestamp.from(Instant.now());

            writeJdbcTemplate.update(update, authentication.getName(), name, eTag, bytes, now, now);

            return eTag;
        } catch (final DataAccessException e) {
            throw new UpstreamDependencyException("Unable to communicate successfully with database.", e);
        }
    }

    @Override
    public Resource getDocumentResource(final Authentication authentication, final String name, final String eTag, final String xRequestedFor) {
        final StringBuilder stringBuilder =
                new StringBuilder().append("SELECT * FROM document WHERE principal = ? AND name = ?");

        final ArrayList<Object> params = new ArrayList<>();

        final String principal = StringUtils.isEmpty(xRequestedFor) ? authentication.getName() : xRequestedFor;

        params.add(principal);
        params.add(name);

        if (eTag != null && !eTag.isEmpty()) {
            stringBuilder.append(" AND e_tag = ?");
            params.add(eTag);
        }

        final String query = stringBuilder.append(" ORDER BY created_at DESC LIMIT 1;").toString();

        try {
            final Document document =
                    readJdbcTemplate.queryForObject(query,
                                                    params.toArray(),
                                                    this::documentRowMapper);

            if (document == null) {
                throw new ResourceNotFoundException("The provided resource does not exist.");
            }

            return new ByteArrayResource(document.getContent());
        } catch (final IncorrectResultSizeDataAccessException e) {
            throw new ResourceNotFoundException("The provided resource does not exist.");
        } catch (final DataAccessException e) {
            throw new UpstreamDependencyException("Unable to communicate with database.", e);
        }
    }

    @Override
    public Collection<DocumentDto> listDocuments(final Authentication authentication) {
        final String query = "SELECT principal, name, e_tag, NULL AS content, created_at, updated_at FROM document WHERE principal = ?;";

        try {
            final Collection<Document> documents =
                    readJdbcTemplate.query(query,
                                           new Object[]{authentication.getName()},
                                           this::documentRowMapper);

            return documentMapper.asDto(documents);
        } catch (final DataAccessException e) {
            throw new UpstreamDependencyException("Unable to communicate with database.", e);
        }
    }

    @Override
    @Transactional
    public void lockDocument(final String name, final String eTag, final String xRequestedFor, final String xLockId) {
        final String query = "INSERT IGNORE INTO lock (sid, principal, name, e_tag) VALUES (?, ?, ?, ?);";

        try {
            writeJdbcTemplate.update(query, xLockId, xRequestedFor, name, eTag);
        } catch (final DataAccessException e) {
            throw new UpstreamDependencyException("Unable to communicate with database.", e);
        }
    }

    @Override
    @Transactional
    public void unlockDocument(final String name, final String eTag, final String xRequestedFor, final String xLockId) {
        final String query = "DELETE FROM lock WHERE sid = ? AND principal = ? AND name = ? AND e_tag = ?;";

        try {
            writeJdbcTemplate.update(query, xLockId, xRequestedFor, name, eTag);
        } catch (final DataAccessException e) {
            throw new UpstreamDependencyException("Unable to communicate with database.", e);
        }
    }

    @Override
    @Transactional
    public void lockDocument(final DocumentSubmittedEvent documentSubmittedEvent) {
        try {
            lockDocument(documentSubmittedEvent.getName(),
                         documentSubmittedEvent.getETag(),
                         documentSubmittedEvent.getPrincipal(),
                         documentSubmittedEvent.getId());

            final Future<RecordMetadata> future =
                    documentLockedEventKafkaProducer.send(new ProducerRecord<>("document-locked",
                                                                               DocumentLockedEvent.newBuilder()
                                                                                                  .setCreatedAt(Instant.now().toEpochMilli())
                                                                                                  .setETag(documentSubmittedEvent.getETag())
                                                                                                  .setName(documentSubmittedEvent.getName())
                                                                                                  .setPrincipal(documentSubmittedEvent.getPrincipal())
                                                                                                  .setId(documentSubmittedEvent.getId())
                                                                                                  .build()));

            future.get();
        } catch (final ResourceNotFoundException e) {
            handleError(documentSubmittedEvent, e);
        } catch (final InterruptedException | ExecutionException e) {
            throw new InternalFailureException("Unable to publish document locked event.", e);
        }
    }

    @Override
    @Transactional
    public void unlockDocument(final SubmissionCancelledEvent submissionCancelledEvent) {
        try {
            unlockDocument(submissionCancelledEvent.getName(),
                           submissionCancelledEvent.getETag(),
                           submissionCancelledEvent.getPrincipal(),
                           submissionCancelledEvent.getId());

            final Future<RecordMetadata> future =
                    documentUnlockedEventKafkaProducer.send(new ProducerRecord<>("document-unlocked",
                                                                                 DocumentUnlockedEvent.newBuilder()
                                                                                                      .setCreatedAt(Instant.now().toEpochMilli())
                                                                                                      .setETag(submissionCancelledEvent.getETag())
                                                                                                      .setName(submissionCancelledEvent.getName())
                                                                                                      .setPrincipal(submissionCancelledEvent.getPrincipal())
                                                                                                      .setId(submissionCancelledEvent.getId())
                                                                                                      .build()));

            future.get();
        } catch (final ResourceNotFoundException e) {
            handleError(submissionCancelledEvent, e);
        } catch (final InterruptedException | ExecutionException e) {
            throw new InternalFailureException("Unable to publish document unlocked event.", e);
        }
    }

    private Document documentRowMapper(final ResultSet resultSet, final int ignored) throws SQLException {
        return Document.builder()
                       .principal(resultSet.getString("principal"))
                       .content(resultSet.getBytes("content"))
                       .createdAt(resultSet.getTimestamp("created_at"))
                       .updatedAt(resultSet.getTimestamp("updated_at"))
                       .name(resultSet.getString("name"))
                       .eTag(resultSet.getString("e_tag"))
                       .build();
    }

    private void handleError(final DocumentSubmittedEvent documentSubmittedEvent,
                             final Throwable cause) {
        try {
            final Future<RecordMetadata> future =
                    documentLockedEventKafkaProducer.send(new ProducerRecord<>("document-locked",
                                                                               DocumentLockedEvent.newBuilder()
                                                                                                  .setCreatedAt(Instant.now().toEpochMilli())
                                                                                                  .setETag(documentSubmittedEvent.getETag())
                                                                                                  .setName(documentSubmittedEvent.getName())
                                                                                                  .setPrincipal(documentSubmittedEvent.getPrincipal())
                                                                                                  .setId(documentSubmittedEvent.getId())
                                                                                                  .setErrorCode(cause.getClass().getSimpleName())
                                                                                                  .setErrorMessage(cause.getMessage())
                                                                                                  .build()));
            future.get();
        } catch (final InterruptedException | ExecutionException e) {
            throw new InternalFailureException("Unable to publish document locked event.", e);
        }
    }

    private void handleError(final SubmissionCancelledEvent submissionCancelledEvent,
                             final Throwable cause) {
        try {
            final Future<RecordMetadata> future =
                    documentUnlockedEventKafkaProducer.send(new ProducerRecord<>("document-unlocked",
                                                                                 DocumentUnlockedEvent.newBuilder()
                                                                                                      .setCreatedAt(Instant.now().toEpochMilli())
                                                                                                      .setETag(submissionCancelledEvent.getETag())
                                                                                                      .setName(submissionCancelledEvent.getName())
                                                                                                      .setPrincipal(submissionCancelledEvent.getPrincipal())
                                                                                                      .setId(submissionCancelledEvent.getId())
                                                                                                      .setErrorCode(cause.getClass().getSimpleName())
                                                                                                      .setErrorMessage(cause.getMessage())
                                                                                                      .build()));
            future.get();
        } catch (final InterruptedException | ExecutionException e) {
            throw new InternalFailureException("Unable to publish document unlocked event.", e);
        }
    }
}
