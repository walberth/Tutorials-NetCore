package com.pluralsight.reactive.translation.service;

import com.pluralsight.reactive.translation.exception.UpstreamDependencyException;
import com.pluralsight.reactive.translation.model.domain.RejectedTranslationPoll;
import com.pluralsight.reactive.translation.model.domain.Submission;
import lombok.extern.slf4j.Slf4j;
import org.springframework.dao.DataAccessException;
import org.springframework.dao.DuplicateKeyException;
import org.springframework.dao.IncorrectResultSizeDataAccessException;
import org.springframework.jdbc.core.JdbcTemplate;

import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.time.Instant;
import java.time.temporal.ChronoUnit;
import java.util.Collection;

@Slf4j
public class RejectedTranslationPoller implements Runnable {

    private final String start;
    private final String end;
    private final long threshold;
    private final int limit;
    private final String id;
    private final SubmissionService submissionService;
    private final JdbcTemplate readJdbcTemplate;
    private final JdbcTemplate writeJdbcTemplate;

    public RejectedTranslationPoller(final String start,
                                     final String end,
                                     final long threshold,
                                     final int limit,
                                     final String id,
                                     final SubmissionService submissionService,
                                     final JdbcTemplate readJdbcTemplate,
                                     final JdbcTemplate writeJdbcTemplate) {
        this.start = start;
        this.end = end;
        this.threshold = threshold;
        this.limit = limit;
        this.id = id;
        this.submissionService = submissionService;
        this.readJdbcTemplate = readJdbcTemplate;
        this.writeJdbcTemplate = writeJdbcTemplate;
    }

    @Override
    public void run() {
        while (true) {
            try {
                Thread.sleep(threshold * 60000);

                final RejectedTranslationPoll rejectedTranslationPoll = getRejectedTranslationPoll(start, end);
                final Timestamp lease = Timestamp.from(Instant.now().minus(threshold, ChronoUnit.MINUTES));

                if (rejectedTranslationPoll != null &&
                    !rejectedTranslationPoll.getWorker().equals(id) &&
                    rejectedTranslationPoll.getLease().before(lease) &&
                    updateClaim(start, end, lease, id, null) == 0) {
                    continue;
                } else if (rejectedTranslationPoll == null && insertClaim(start, end, id) == 0) {
                    continue;
                } else if (rejectedTranslationPoll != null &&
                           rejectedTranslationPoll.getWorker().equals(id) &&
                           updateClaim(start, end, lease, id, null) == 0) {
                    continue;
                }

                final Collection<Submission> submissions =
                        submissionService.listPendingAcceptanceSubmissions(start,
                                                                           end,
                                                                           threshold,
                                                                           rejectedTranslationPoll == null ? null : rejectedTranslationPoll.getAfter(),
                                                                           limit);

                log.info("Found {} records.", submissions.size());

                for (final Submission submission : submissions) {
                    submissionService.startRollback(submission.getId());
                    updateClaim(start, end, Timestamp.from(Instant.now().minus(threshold, ChronoUnit.MINUTES)), id, submission.getName());
                }
            } catch (final RuntimeException e) {
                log.error("Caught exception while processing records.", e);
            } catch (final InterruptedException e) {
                break;
            }
        }
    }

    private RejectedTranslationPoll getRejectedTranslationPoll(final String start, final String end) {
        final String query = "SELECT * FROM rejected_translation_poll WHERE start = ? AND end = ?;";

        try {
            return readJdbcTemplate.queryForObject(query,
                                                   new Object[]{start, end},
                                                   this::rejectedTranslationPollRowMapper);
        } catch (final IncorrectResultSizeDataAccessException e) {
            return null;
        } catch (final DataAccessException e) {
            throw new UpstreamDependencyException("Unable to communicate with database.", e);
        }
    }

    private int insertClaim(final String start, final String end, final String worker) {
        final String query = "INSERT INTO rejected_translation_poll (start, end, worker, lease) VALUES (?, ?, ?, ?);";

        try {
            return writeJdbcTemplate.update(query, start, end, worker, Timestamp.from(Instant.now()));
        } catch (final DuplicateKeyException e) {
            return 0;
        } catch (final DataAccessException e) {
            throw new UpstreamDependencyException("Unable to communicate with database.", e);
        }
    }

    private int updateClaim(final String start, final String end, final Timestamp lease, final String worker, final String after) {
        final StringBuilder query =
                new StringBuilder().append("UPDATE rejected_translation_poll SET worker = ?, lease = ?");

        if (after != null) {
            query.append(", after = ? ");
        }

        query.append("WHERE start = ? AND end = ? AND lease = ?;");

        try {
            return writeJdbcTemplate.update(query.toString(), worker, lease, start, end, lease);
        } catch (final DataAccessException e) {
            throw new UpstreamDependencyException("Unable to communicate with database.", e);
        }
    }

    private RejectedTranslationPoll rejectedTranslationPollRowMapper(final ResultSet resultSet, final int ignored) throws SQLException {
        return RejectedTranslationPoll.builder()
                                      .start(resultSet.getString("start"))
                                      .end(resultSet.getString("end"))
                                      .worker(resultSet.getString("worker"))
                                      .after(resultSet.getString("after"))
                                      .lease(resultSet.getTimestamp("lease"))
                                      .build();
    }
}
