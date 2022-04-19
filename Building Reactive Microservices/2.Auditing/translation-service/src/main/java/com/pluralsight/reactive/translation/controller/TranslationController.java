package com.pluralsight.reactive.translation.controller;

import com.pluralsight.reactive.translation.exception.InternalFailureException;
import com.pluralsight.reactive.translation.exception.UpstreamDependencyException;
import com.pluralsight.reactive.translation.model.api.RegistrationRequest;
import com.pluralsight.reactive.translation.service.TranslationService;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.server.ResponseStatusException;

import javax.validation.Valid;

@Slf4j
@RestController
public class TranslationController {

    private final TranslationService translationService;

    @Autowired
    public TranslationController(final TranslationService translationService) {
        this.translationService = translationService;
    }


    @PreAuthorize(value = "hasRole('translator')")
    @PutMapping(value = "/register", consumes = MediaType.APPLICATION_JSON_VALUE)
    public ResponseEntity<Void> registerTranslator(final Authentication authentication,
                                                   @Valid @RequestBody final RegistrationRequest registrationRequest) {
        try {
            translationService.registerTranslator(authentication, registrationRequest.getLanguages());
            return ResponseEntity.noContent().build();
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
