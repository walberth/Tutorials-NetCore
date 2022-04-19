package com.pluralsight.reactive.translation.service;

import com.pluralsight.reactive.translation.model.dto.LanguagePairDto;
import com.pluralsight.reactive.translation.model.dto.SubmissionDto;
import org.springframework.security.core.Authentication;

import java.util.Collection;

public interface TranslationService {

    void registerTranslator(final Authentication authentication, final Collection<LanguagePairDto> languages);

    void notifyTranslators(final SubmissionDto translation);

    void notifyTranslators(final String id, final String source, final String target);

    void assignTranslator(final Authentication authentication, final SubmissionDto submission);
}
