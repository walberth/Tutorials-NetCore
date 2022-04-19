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
public class RejectedTranslationPoll {
    private String start;
    private String end;
    private String worker;
    private String after;
    private Timestamp lease;
}
