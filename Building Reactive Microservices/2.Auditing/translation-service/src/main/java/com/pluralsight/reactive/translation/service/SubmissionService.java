package com.pluralsight.reactive.translation.service;

import com.pluralsight.reactive.model.domain.DocumentLockedEvent;
import com.pluralsight.reactive.model.domain.DocumentUnlockedEvent;
import com.pluralsight.reactive.translation.model.domain.Submission;
import com.pluralsight.reactive.translation.model.dto.SubmissionDto;
import org.springframework.security.core.Authentication;

import java.util.Collection;

public interface SubmissionService {

    void startTransaction(final Authentication authentication, final String name, final String eTag, final String source, final String target, final long completionDate);

    SubmissionDto getSubmission(final Authentication authentication, final String id);

    void startRollback(final Authentication authentication, final String id);

    void startRollback(final String id);

    void handleDocumentLockedEvent(final DocumentLockedEvent documentLockedEvent);

    void handleDocumentUnlockedEvent(final DocumentUnlockedEvent documentUnlockedEvent);

    Collection<SubmissionDto> listSubmissions(final Authentication authentication);

    Collection<Submission> listPendingLockSubmissions(final String start,
                                                      final String end,
                                                      final long threshold,
                                                      final String after,
                                                      final int limit);

    Collection<Submission> listPendingAcceptanceSubmissions(final String start,
                                                            final String end,
                                                            final long threshold,
                                                            final String after,
                                                            final int limit);

    void acceptSubmission(final Authentication authentication, final String id);
}
