package com.pluralsight.reactive.translation.service;

import com.pluralsight.reactive.model.domain.DocumentLockedEvent;
import lombok.extern.slf4j.Slf4j;
import org.apache.kafka.clients.consumer.ConsumerRecord;

import java.util.Properties;

@Slf4j
public class DocumentLockedEventConsumerLoop extends ConsumerLoop<String, DocumentLockedEvent> {

    private final SubmissionService submissionService;

    public DocumentLockedEventConsumerLoop(final SubmissionService submissionService,
                                           final String topic,
                                           final Properties properties) {
        super(topic, properties);

        this.submissionService = submissionService;
    }

    @Override
    public void handleRecord(final ConsumerRecord<String, DocumentLockedEvent> consumerRecord) {
        submissionService.handleDocumentLockedEvent(consumerRecord.value());
    }
}
