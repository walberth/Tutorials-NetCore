package com.pluralsight.reactive.translation.exception;

public class TranslationException extends RuntimeException {

    public TranslationException(final String message) {
        super(message);
    }

    public TranslationException(final String message, final Throwable cause) {
        super(message, cause);
    }

    public TranslationException(final Throwable cause) {
        super(cause);
    }
}
