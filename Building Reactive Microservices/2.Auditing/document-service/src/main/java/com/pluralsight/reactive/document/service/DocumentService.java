package com.pluralsight.reactive.document.service;

import com.pluralsight.reactive.document.model.dto.DocumentDto;
import com.pluralsight.reactive.model.domain.*;
import org.springframework.core.io.Resource;
import org.springframework.security.core.Authentication;

import java.util.Collection;

public interface DocumentService {

    String uploadDocument(final Authentication authentication, final String name, final byte[] bytes);

    Resource getDocumentResource(final Authentication authentication, final String name, final String eTag, final String xRequestedFor);

    Collection<DocumentDto> listDocuments(final Authentication authentication);

    void lockDocument(final String name, final String eTag, final String xRequestedFor, final String xLockId);

    void unlockDocument(final String name, final String eTag, final String xRequestedFor, final String xLockId);

    void lockDocument(final DocumentSubmittedEvent documentSubmittedEvent);

    void unlockDocument(final SubmissionCancelledEvent submissionCancelledEvent);
}
