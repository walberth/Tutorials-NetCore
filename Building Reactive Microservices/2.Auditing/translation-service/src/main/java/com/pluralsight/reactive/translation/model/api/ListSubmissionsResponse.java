package com.pluralsight.reactive.translation.model.api;

import com.pluralsight.reactive.translation.model.dto.SubmissionDto;
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
public class ListSubmissionsResponse {

    @Builder.Default
    private Collection<SubmissionDto> submissions = Collections.emptyList();
}
