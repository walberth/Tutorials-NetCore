package com.pluralsight.reactive.document.service;

import com.pluralsight.reactive.model.domain.SubmissionCancelledEvent;
import lombok.extern.slf4j.Slf4j;
import org.apache.kafka.clients.consumer.ConsumerRecord;

import java.util.Properties;

@Slf4j
public class SubmissionCancelledEventConsumerLoop extends ConsumerLoop<String, SubmissionCancelledEvent> {

    private final DocumentService documentService;

    public SubmissionCancelledEventConsumerLoop(final DocumentService documentService,
                                                final String topic,
                                                final Properties properties) {
        super(topic, properties);

        this.documentService = documentService;
    }

    @Override
    public void handleRecord(final ConsumerRecord<String, SubmissionCancelledEvent> consumerRecord) {
        documentService.unlockDocument(consumerRecord.value());
    }
}
