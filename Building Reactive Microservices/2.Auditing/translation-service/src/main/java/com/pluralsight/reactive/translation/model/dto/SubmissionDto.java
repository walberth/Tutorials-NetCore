package com.pluralsight.reactive.translation.model.dto;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class SubmissionDto {
    private String id;
    private String name;
    private String eTag;
    private String source;
    private String target;
    private Long completionDate;
    private String status;
    private String details;
    private Long createdAt;
    private Long updatedAt;
}
