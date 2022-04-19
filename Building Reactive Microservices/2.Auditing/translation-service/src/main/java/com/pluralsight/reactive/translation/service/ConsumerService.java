package com.pluralsight.reactive.translation.service;

import com.pluralsight.reactive.model.domain.DocumentLockedEvent;
import com.pluralsight.reactive.model.domain.DocumentUnlockedEvent;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Profile;
import org.springframework.core.task.TaskExecutor;
import org.springframework.stereotype.Service;

@Service
@Profile(value = "m04 | m05")
public class ConsumerService {

    @Autowired
    public ConsumerService(final TaskExecutor taskExecutor,
                           final ConsumerLoop<String, DocumentLockedEvent> documentLockedEventConsumerLoop,
                           final ConsumerLoop<String, DocumentUnlockedEvent> documentUnlockedEventConsumerLoop) {
        taskExecutor.execute(documentLockedEventConsumerLoop);
        taskExecutor.execute(documentUnlockedEventConsumerLoop);
    }
}
