package com.pluralsight.reactive.translation.exception;

public class ResourceNotFoundException extends TranslationException {

    public ResourceNotFoundException(final String message) {
        super(message);
    }

    public ResourceNotFoundException(final String message, final Throwable cause) {
        super(message, cause);
    }

    public ResourceNotFoundException(final Throwable cause) {
        super(cause);
    }
}
