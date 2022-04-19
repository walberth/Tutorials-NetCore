package com.pluralsight.reactive.translation.constant;

import lombok.experimental.UtilityClass;

@UtilityClass
public class SubmissionStatus {
    public static final String PENDING_LOCK = "PENDING_LOCK";
    public static final String PENDING_CANCELLATION = "PENDING_CANCELLATION";
    public static final String PENDING_ACCEPTANCE = "PENDING_ACCEPTANCE";
    public static final String ACCEPTED = "ACCEPTED";
    public static final String CANCELLED = "CANCELLED";
}
