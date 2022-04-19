package com.pluralsight.reactive.translation.service;

import com.pluralsight.reactive.translation.exception.PreconditionFailedException;
import com.pluralsight.reactive.translation.exception.UpstreamDependencyException;
import com.pluralsight.reactive.translation.model.domain.Translator;
import com.pluralsight.reactive.translation.model.dto.LanguagePairDto;
import com.pluralsight.reactive.translation.model.dto.SubmissionDto;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.dao.DataAccessException;
import org.springframework.dao.DuplicateKeyException;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.core.Authentication;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.time.Instant;
import java.util.Collection;
import java.util.Collections;
import java.util.List;
import java.util.Random;

@Slf4j
@Service
public class SimpleTranslationService implements TranslationService {

    private final JdbcTemplate readJdbcTemplate;
    private final JdbcTemplate writeJdbcTemplate;

    @Autowired
    public SimpleTranslationService(@Qualifier(value = "ReadJdbcTemplate") final JdbcTemplate readJdbcTemplate,
                                    @Qualifier(value = "WriteJdbcTemplate") final JdbcTemplate writeJdbcTemplate) {
        this.readJdbcTemplate = readJdbcTemplate;
        this.writeJdbcTemplate = writeJdbcTemplate;
    }

    @Override
    @Transactional
    public void registerTranslator(final Authentication authentication,
                                   final Collection<LanguagePairDto> languages) {
        for (final LanguagePairDto language : languages) {
            registerTranslator(authentication, language);
        }
    }

    private void registerTranslator(final Authentication authentication,
                                    final LanguagePairDto language) {
        final String query = "INSERT IGNORE INTO translator (principal, source, target, created_at) VALUES (?, ?, ?, ?);";

        try {
            writeJdbcTemplate.update(query,
                                     authentication.getName(),
                                     language.getSource(),
                                     language.getTarget(),
                                     Timestamp.from(Instant.now()));
        } catch (final DataAccessException e) {
            throw new UpstreamDependencyException("Unable to communicate successfully with database.", e);
        }
    }

    @Override
    @Transactional
    public void notifyTranslators(final SubmissionDto submission) {
        notifyTranslators(submission.getId(), submission.getSource(), submission.getTarget());
    }

    @Override
    @Transactional
    public void notifyTranslators(final String id, final String source, final String target) {
        final String query = "SELECT principal FROM translator WHERE source = ? AND target = ?;";
        final Random random = new Random();

        try {
            final List<String> translators =
                    readJdbcTemplate.queryForList(query,
                                                  new Object[]{source, target},
                                                  String.class);

            if (translators.size() == 0) {
                throw new PreconditionFailedException("A translator was not found for the provided language.");
            }

            log.info("Chose translator, {}, for submission, {}.",
                     translators.get(random.nextInt(translators.size())),
                     id);
        } catch (final DataAccessException e) {
            throw new UpstreamDependencyException("Unable to communicate with database.", e);
        }
    }

    @Override
    @Transactional
    public void assignTranslator(final Authentication authentication, final SubmissionDto submission) {
        final String query = "INSERT INTO assignment (sid, principal, created_at) VALUES (?, ?, ?);";

        try {
            writeJdbcTemplate.update(query,
                                     submission.getId(),
                                     authentication.getName(),
                                     Timestamp.from(Instant.now()));
        } catch (final DuplicateKeyException e) {
            throw new PreconditionFailedException("A translator has already been assigned to this task.", e);
        } catch (final DataAccessException e) {
            throw new UpstreamDependencyException("Unable to communicate successfully with database.", e);
        }
    }

    private Translator registrantRowMapper(final ResultSet resultSet, final int ignored) throws SQLException {
        return Translator.builder()
                         .principal(resultSet.getString("principal"))
                         .languages(Collections.singletonList(LanguagePairDto.builder()
                                                                             .source(resultSet.getString("source"))
                                                                             .target(resultSet.getString("target"))
                                                                             .build()))
                         .build();
    }
}
