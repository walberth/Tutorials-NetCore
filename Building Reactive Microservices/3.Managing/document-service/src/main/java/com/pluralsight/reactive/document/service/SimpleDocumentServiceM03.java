package com.pluralsight.reactive.document.service;

import com.pluralsight.reactive.document.exception.ResourceNotFoundException;
import com.pluralsight.reactive.document.exception.UpstreamDependencyException;
import com.pluralsight.reactive.document.mapper.DocumentMapper;
import com.pluralsight.reactive.document.model.domain.Document;
import com.pluralsight.reactive.document.model.dto.DocumentDto;
import com.pluralsight.reactive.model.domain.DocumentSubmittedEvent;
import com.pluralsight.reactive.model.domain.SubmissionCancelledEvent;
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

@Service
@Profile(value = "m03")
public class SimpleDocumentServiceM03 implements DocumentService {

    private final JdbcTemplate readJdbcTemplate;
    private final JdbcTemplate writeJdbcTemplate;
    private final DocumentMapper documentMapper;

    @Autowired
    public SimpleDocumentServiceM03(@Qualifier(value = "ReadJdbcTemplate") final JdbcTemplate readJdbcTemplate,
                                    @Qualifier(value = "WriteJdbcTemplate") final JdbcTemplate writeJdbcTemplate,
                                    final DocumentMapper documentMapper) {
        this.readJdbcTemplate = readJdbcTemplate;
        this.writeJdbcTemplate = writeJdbcTemplate;
        this.documentMapper = documentMapper;
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
        throw new UnsupportedOperationException("Not implemented in this module.");
    }

    @Override
    @Transactional
    public void unlockDocument(final SubmissionCancelledEvent submissionCancelledEvent) {
        throw new UnsupportedOperationException("Not implemented in this module.");
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
}
