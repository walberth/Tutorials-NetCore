package com.pluralsight.reactive.document.exception;

public class DocumentException extends RuntimeException {

    public DocumentException(final String message) {
        super(message);
    }

    public DocumentException(final String message, final Throwable cause) {
        super(message, cause);
    }

    public DocumentException(final Throwable cause) {
        super(cause);
    }
}
