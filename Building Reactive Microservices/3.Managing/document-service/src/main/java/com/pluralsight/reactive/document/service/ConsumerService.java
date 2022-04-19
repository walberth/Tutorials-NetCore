package com.pluralsight.reactive.document.service;

import com.pluralsight.reactive.model.domain.DocumentSubmittedEvent;
import com.pluralsight.reactive.model.domain.SubmissionCancelledEvent;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Profile;
import org.springframework.core.task.TaskExecutor;
import org.springframework.stereotype.Service;

@Service
@Profile(value = "m04 | m05")
public class ConsumerService {

    @Autowired
    public ConsumerService(final TaskExecutor taskExecutor,
                           final ConsumerLoop<String, DocumentSubmittedEvent> documentSubmittedEventConsumerLoop,
                           final ConsumerLoop<String, SubmissionCancelledEvent> submissionCancelledEventConsumerLoop) {
        taskExecutor.execute(documentSubmittedEventConsumerLoop);
        taskExecutor.execute(submissionCancelledEventConsumerLoop);
    }
}

