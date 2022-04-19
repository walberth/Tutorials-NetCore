package com.pluralsight.reactive.translation.model.api;

import com.pluralsight.reactive.translation.model.dto.SubmissionDto;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class GetSubmissionResponse {
    private SubmissionDto data;
}
