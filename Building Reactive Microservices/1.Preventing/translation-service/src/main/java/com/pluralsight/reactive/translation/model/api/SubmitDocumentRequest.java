package com.pluralsight.reactive.translation.model.api;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import javax.validation.constraints.NotBlank;
import javax.validation.constraints.NotNull;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class SubmitDocumentRequest {

    @NotBlank(message = "A valid name must be defined")
    private String name;

    @NotBlank(message = "A valid ETag must be defined")
    private String etag;

    @NotBlank(message = "A valid source language must be defined")
    private String source;

    @NotBlank(message = "A valid target language must be defined")
    private String target;

    @NotNull(message = "A valid completion data must be defined")
    private Long completionDate;
}
