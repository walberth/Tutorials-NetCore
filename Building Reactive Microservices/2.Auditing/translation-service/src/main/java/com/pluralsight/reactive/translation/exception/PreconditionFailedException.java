package com.pluralsight.reactive.translation.exception;

public class PreconditionFailedException extends TranslationException {

    public PreconditionFailedException(final String message) {
        super(message);
    }

    public PreconditionFailedException(final String message, final Throwable cause) {
        super(message, cause);
    }

    public PreconditionFailedException(final Throwable cause) {
        super(cause);
    }
}
