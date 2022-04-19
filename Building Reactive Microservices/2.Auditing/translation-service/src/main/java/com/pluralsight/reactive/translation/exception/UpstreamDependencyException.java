package com.pluralsight.reactive.translation.exception;

public class UpstreamDependencyException extends TranslationException {

    public UpstreamDependencyException(final String message) {
        super(message);
    }

    public UpstreamDependencyException(final String message, final Throwable cause) {
        super(message, cause);
    }

    public UpstreamDependencyException(final Throwable cause) {
        super(cause);
    }
}
