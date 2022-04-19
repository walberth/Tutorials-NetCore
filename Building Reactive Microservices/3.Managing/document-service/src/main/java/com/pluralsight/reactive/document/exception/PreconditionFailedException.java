package com.pluralsight.reactive.document.exception;

public class PreconditionFailedException extends DocumentException {

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
