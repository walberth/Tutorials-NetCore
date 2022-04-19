package com.pluralsight.reactive.translation.exception;

public class InternalFailureException extends TranslationException {

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
