package com.pluralsight.reactive.document.model.api;

import com.pluralsight.reactive.document.model.dto.DocumentDto;
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
public class ListDocumentsResponse {

    @Builder.Default
    private Collection<DocumentDto> documents = Collections.emptyList();
}
