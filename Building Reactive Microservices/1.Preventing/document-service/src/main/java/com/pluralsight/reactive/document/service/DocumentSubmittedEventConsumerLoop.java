package com.pluralsight.reactive.document.service;

import com.pluralsight.reactive.model.domain.DocumentSubmittedEvent;
import lombok.extern.slf4j.Slf4j;
import org.apache.kafka.clients.consumer.ConsumerRecord;

import java.util.Properties;

@Slf4j
public class DocumentSubmittedEventConsumerLoop extends ConsumerLoop<String, DocumentSubmittedEvent> {

    private final DocumentService documentService;

    public DocumentSubmittedEventConsumerLoop(final DocumentService documentService,
                                              final String topic,
                                              final Properties properties) {
        super(topic, properties);

        this.documentService = documentService;
    }

    @Override
    public void handleRecord(final ConsumerRecord<String, DocumentSubmittedEvent> consumerRecord) {
        documentService.lockDocument(consumerRecord.value());
    }
}
