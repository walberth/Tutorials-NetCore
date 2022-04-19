package com.pluralsight.reactive.document.model.domain;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.sql.Timestamp;

@Data
@Builder
@NoArgsConstructor
@AllArgsConstructor
public class Document {
    private String principal;
    private String name;
    private String eTag;
    private byte[] content;
    private Timestamp createdAt;
    private Timestamp updatedAt;
}
