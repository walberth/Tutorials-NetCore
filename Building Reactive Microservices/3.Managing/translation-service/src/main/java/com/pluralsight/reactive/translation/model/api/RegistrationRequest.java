package com.pluralsight.reactive.translation.model.api;

import com.pluralsight.reactive.translation.model.dto.LanguagePairDto;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import javax.validation.constraints.NotEmpty;
import java.util.Collection;
import java.util.Collections;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class RegistrationRequest {

    @Builder.Default
    @NotEmpty(message = "A valid set of language pairs must be defined")
    private Collection<LanguagePairDto> languages = Collections.emptyList();
}
