package com.pluralsight.reactive.translation.model.dto;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.Collection;
import java.util.Collections;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class TranslatorDto {

    private String principal;

    @Builder.Default
    private Collection<LanguagePairDto> languages = Collections.emptyList();
}
