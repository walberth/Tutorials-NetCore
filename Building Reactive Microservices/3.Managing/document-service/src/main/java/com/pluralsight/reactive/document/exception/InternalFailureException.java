package com.pluralsight.reactive.document.exception;

public class InternalFailureException extends DocumentException {

    public InternalFailureException(final String message) {
        super(message);
    }

    public InternalFailureException(final String message, final Throwable cause) {
        super(message, cause);
    }

    public InternalFailureException(final Throwable cause) {
        super(cause);
    }
}
