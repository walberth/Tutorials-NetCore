package com.pluralsight.reactive.translation.service;

import com.pluralsight.reactive.model.domain.DocumentUnlockedEvent;
import lombok.extern.slf4j.Slf4j;
import org.apache.kafka.clients.consumer.ConsumerRecord;

import java.util.Properties;

@Slf4j
public class DocumentUnlockedEventConsumerLoop extends ConsumerLoop<String, DocumentUnlockedEvent> {

    private final SubmissionService submissionService;

    public DocumentUnlockedEventConsumerLoop(final SubmissionService submissionService,
                                             final String topic,
                                             final Properties properties) {
        super(topic, properties);

        this.submissionService = submissionService;
    }

    @Override
    public void handleRecord(final ConsumerRecord<String, DocumentUnlockedEvent> consumerRecord) {
        submissionService.handleDocumentUnlockedEvent(consumerRecord.value());
    }
}
