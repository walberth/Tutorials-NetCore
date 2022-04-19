package com.pluralsight.reactive.translation.controller;

import com.pluralsight.reactive.translation.exception.InternalFailureException;
import com.pluralsight.reactive.translation.exception.PreconditionFailedException;
import com.pluralsight.reactive.translation.exception.ResourceNotFoundException;
import com.pluralsight.reactive.translation.exception.UpstreamDependencyException;
import com.pluralsight.reactive.translation.model.api.GetSubmissionResponse;
import com.pluralsight.reactive.translation.model.api.ListSubmissionsResponse;
import com.pluralsight.reactive.translation.model.api.SubmitDocumentRequest;
import com.pluralsight.reactive.translation.model.dto.SubmissionDto;
import com.pluralsight.reactive.translation.service.SubmissionService;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ResponseStatusException;

import javax.validation.Valid;
import java.util.Collection;

@Slf4j
@RestController
public class SubmissionController {

    private final SubmissionService submissionService;

    @Autowired
    public SubmissionController(final SubmissionService submissionService) {
        this.submissionService = submissionService;
    }

    @PostMapping(value = "/submissions", consumes = MediaType.APPLICATION_JSON_VALUE)
    public ResponseEntity<Void> submitDocument(final Authentication authentication,
                                               @Valid @RequestBody final SubmitDocumentRequest submitDocumentRequest) {
        try {
            submissionService.startTransaction(authentication,
                                               submitDocumentRequest.getName(),
                                               submitDocumentRequest.getEtag(),
                                               submitDocumentRequest.getSource(),
                                               submitDocumentRequest.getTarget(),
                                               submitDocumentRequest.getCompletionDate());

            return ResponseEntity.accepted().build();
        } catch (final PreconditionFailedException e) {
            log.error("Caught a known exception while processing request.", e);
            throw new ResponseStatusException(HttpStatus.PRECONDITION_FAILED, "The necessary precondition failed.");
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

    @PreAuthorize("hasRole('translator')")
    @PostMapping(value = "/submissions/{id}/accept")
    public ResponseEntity<Void> acceptSubmission(final Authentication authentication,
                                                 @PathVariable(value = "id") final String id) {
        try {
            submissionService.acceptSubmission(authentication, id);
            return ResponseEntity.noContent().build();
        } catch (final ResourceNotFoundException e) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "The specified resource does not exist.", e);
        } catch (final PreconditionFailedException e) {
            log.error("Caught a known exception while processing request.", e);
            throw new ResponseStatusException(HttpStatus.PRECONDITION_FAILED, "The necessary precondition failed.");
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

    @GetMapping(value = "/submissions", produces = MediaType.APPLICATION_JSON_VALUE)
    public ResponseEntity<ListSubmissionsResponse> listSubmissions(final Authentication authentication) {
        try {
            final Collection<SubmissionDto> submissions = submissionService.listSubmissions(authentication);
            return ResponseEntity.ok().body(ListSubmissionsResponse.builder().submissions(submissions).build());
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

    @GetMapping(value = "/submissions/{id}", produces = MediaType.APPLICATION_JSON_VALUE)
    public ResponseEntity<GetSubmissionResponse> getSubmissions(final Authentication authentication,
                                                                @PathVariable(value = "id") final String id) {
        try {
            final SubmissionDto submission = submissionService.getSubmission(authentication, id);
            return ResponseEntity.ok().body(GetSubmissionResponse.builder().data(submission).build());
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

    @DeleteMapping(value = "/submissions/{id}")
    public ResponseEntity<Void> cancelSubmission(final Authentication authentication,
                                                 @PathVariable(value = "id") final String id) {
        try {
            submissionService.startRollback(authentication, id);
            return ResponseEntity.accepted().build();
        } catch (final ResourceNotFoundException e) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "The specified resource does not exist.", e);
        } catch (final PreconditionFailedException e) {
            log.error("Caught a known exception while processing request.", e);
            throw new ResponseStatusException(HttpStatus.PRECONDITION_FAILED, "The necessary precondition failed.");
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
