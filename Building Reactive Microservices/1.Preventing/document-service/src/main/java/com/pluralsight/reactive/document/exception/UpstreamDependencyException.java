package com.pluralsight.reactive.document.exception;

public class UpstreamDependencyException extends DocumentException {

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
