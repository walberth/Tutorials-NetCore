package com.pluralsight.reactive.translation.model.domain;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.sql.Timestamp;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class Submission {
    private String id;
    private String principal;
    private String name;
    private String eTag;
    private String source;
    private String target;
    private Timestamp completionDate;
    private String status;
    private String details;
    private Timestamp createdAt;
    private Timestamp updatedAt;
}
