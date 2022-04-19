package com.pluralsight.reactive.document.controller;

import com.pluralsight.reactive.document.constant.Headers;
import com.pluralsight.reactive.document.exception.InternalFailureException;
import com.pluralsight.reactive.document.exception.ResourceNotFoundException;
import com.pluralsight.reactive.document.exception.UpstreamDependencyException;
import com.pluralsight.reactive.document.model.api.ListDocumentsResponse;
import com.pluralsight.reactive.document.model.dto.DocumentDto;
import com.pluralsight.reactive.document.service.DocumentService;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Profile;
import org.springframework.core.io.Resource;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;
import org.springframework.web.server.ResponseStatusException;

import java.io.IOException;
import java.net.URI;
import java.util.Collection;

@Slf4j
@RestController
@Profile(value = "m03")
public class DocumentControllerM03 {

    private final DocumentService documentService;

    @Autowired
    public DocumentControllerM03(final DocumentService documentService) {
        this.documentService = documentService;
    }

    @PutMapping(value = "/documents", consumes = MediaType.MULTIPART_FORM_DATA_VALUE)
    public ResponseEntity<Void> uploadDocument(final Authentication authentication,
                                               @RequestParam("file") final MultipartFile multipartFile) {
        try {
            final String eTag = documentService.uploadDocument(authentication, multipartFile.getOriginalFilename(), multipartFile.getBytes());
            final URI uri = URI.create(String.format("/documents/%s", multipartFile.getName()));

            return ResponseEntity.created(uri).eTag(eTag).build();
        } catch (final IOException e) {
            log.error("Failed to read input stream from request.", e);
            throw new ResponseStatusException(HttpStatus.INTERNAL_SERVER_ERROR, "Failed to read input stream.");
        } catch (final InternalFailureException e) {
            log.error("Caught a known exception while processing request.", e);
            throw new ResponseStatusException(HttpStatus.INTERNAL_SERVER_ERROR, "An internal error occurred. Please try again.");
        } catch (final UpstreamDependencyException e) {
            log.error("An upstream dependency returned an error while processing the request.", e);
            throw new ResponseStatusException(HttpStatus.BAD_GATEWAY, "An error occurred with an upstream dependency. Please try again.");
        } catch (final Exception e) {
            log.error("An unknown exception occurred while processing the request.", e);
            throw new ResponseStatusException(HttpStatus.INTERNAL_SERVER_ERROR, "Unable to process request. Please try again.");
        }
    }

    @GetMapping(value = "/documents/{name}", produces = MediaType.APPLICATION_OCTET_STREAM_VALUE)
    public ResponseEntity<Resource> getDocument(final Authentication authentication,
                                                @PathVariable(value = "name") final String name,
                                                @RequestHeader(value = HttpHeaders.ETAG, required = false) final String eTag,
                                                @RequestHeader(value = Headers.X_REQUESTED_FOR, required = false) final String xRequestedFor) {
        try {
            final Resource resource = documentService.getDocumentResource(authentication, name, eTag, xRequestedFor);
            final String contentDisposition = String.format("attachment; filename=\"%s\"", resource.getFilename());

            return ResponseEntity.ok()
                                 .contentType(MediaType.APPLICATION_OCTET_STREAM)
                                 .header(HttpHeaders.CONTENT_DISPOSITION, contentDisposition)
                                 .body(resource);
        } catch (final ResourceNotFoundException e) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "The specified resource does not exist.", e);
        } catch (final InternalFailureException e) {
            log.error("Caught a known exception while processing request.", e);
            throw new ResponseStatusException(HttpStatus.INTERNAL_SERVER_ERROR, "An internal error occurred. Please try again.");
        } catch (final UpstreamDependencyException e) {
            log.error("An upstream dependency returned an error while processing the request.", e);
            throw new ResponseStatusException(HttpStatus.BAD_GATEWAY, "An error occurred with an upstream dependency. Please try again.");
        } catch (final Exception e) {
            log.error("An unknown exception occurred while processing the request.", e);
            throw new ResponseStatusException(HttpStatus.INTERNAL_SERVER_ERROR, "Unable to process request. Please try again.");
        }
    }

    @GetMapping(value = "/documents", produces = MediaType.APPLICATION_JSON_VALUE)
    public ResponseEntity<ListDocumentsResponse> listDocuments(final Authentication authentication) {
        try {
            final Collection<DocumentDto> documents = documentService.listDocuments(authentication);
            return ResponseEntity.ok().body(ListDocumentsResponse.builder().documents(documents).build());
        } catch (final ResourceNotFoundException e) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "The specified resource does not exist.", e);
        } catch (final InternalFailureException e) {
            log.error("Caught a known exception while processing request.", e);
            throw new ResponseStatusException(HttpStatus.INTERNAL_SERVER_ERROR, "An internal error occurred. Please try again.");
        } catch (final UpstreamDependencyException e) {
            log.error("An upstream dependency returned an error while processing the request.", e);
            throw new ResponseStatusException(HttpStatus.BAD_GATEWAY, "An error occurred with an upstream dependency. Please try again.");
        } catch (final Exception e) {
            log.error("An unknown exception occurred while processing the request.", e);
            throw new ResponseStatusException(HttpStatus.INTERNAL_SERVER_ERROR, "Unable to process request. Please try again.");
        }
    }

    @PreAuthorize("hasRole('admin')")
    @PostMapping(value = "/documents/{name}/lock")
    public ResponseEntity<Void> lockDocument(@PathVariable(value = "name") final String name,
                                             @RequestHeader(value = HttpHeaders.ETAG) final String eTag,
                                             @RequestHeader(value = Headers.X_REQUESTED_FOR) final String xRequestedFor,
                                             @RequestHeader(value = Headers.X_LOCK_ID) final String xLockId) {
        try {
            documentService.lockDocument(name, eTag, xRequestedFor, xLockId);
            return ResponseEntity.noContent().build();
        } catch (final ResourceNotFoundException e) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "The specified resource does not exist.", e);
        } catch (final InternalFailureException e) {
            log.error("Caught a known exception while processing request.", e);
            throw new ResponseStatusException(HttpStatus.INTERNAL_SERVER_ERROR, "An internal error occurred. Please try again.");
        } catch (final UpstreamDependencyException e) {
            log.error("An upstream dependency returned an error while processing the request.", e);
            throw new ResponseStatusException(HttpStatus.BAD_GATEWAY, "An error occurred with an upstream dependency. Please try again.");
        } catch (final Exception e) {
            log.error("An unknown exception occurred while processing the request.", e);
            throw new ResponseStatusException(HttpStatus.INTERNAL_SERVER_ERROR, "Unable to process request. Please try again.");
        }
    }

    @PreAuthorize("hasRole('admin')")
    @PostMapping(value = "/documents/{name}/unlock")
    public ResponseEntity<Void> unlockDocument(@PathVariable(value = "name") final String name,
                                               @RequestHeader(value = HttpHeaders.ETAG) final String eTag,
                                               @RequestHeader(value = Headers.X_REQUESTED_FOR) final String xRequestedFor,
                                               @RequestHeader(value = Headers.X_LOCK_ID) final String xLockId) {
        try {
            documentService.unlockDocument(name, eTag, xRequestedFor, xLockId);
            return ResponseEntity.noContent().build();
        } catch (final ResourceNotFoundException e) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "The specified resource does not exist.", e);
        } catch (final InternalFailureException e) {
            log.error("Caught a known exception while processing request.", e);
            throw new ResponseStatusException(HttpStatus.INTERNAL_SERVER_ERROR, "An internal error occurred. Please try again.");
        } catch (final UpstreamDependencyException e) {
            log.error("An upstream dependency returned an error while processing the request.", e);
            throw new ResponseStatusException(HttpStatus.BAD_GATEWAY, "An error occurred with an upstream dependency. Please try again.");
        } catch (final Exception e) {
            log.error("An unknown exception occurred while processing the request.", e);
            throw new ResponseStatusException(HttpStatus.INTERNAL_SERVER_ERROR, "Unable to process request. Please try again.");
        }
    }
}
