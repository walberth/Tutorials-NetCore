package com.pluralsight.reactive.translation.service;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Profile;
import org.springframework.core.task.TaskExecutor;
import org.springframework.stereotype.Service;

import java.util.Collection;

@Service
@Profile(value = "m05")
public class RejectedTranslationService {

    @Autowired
    public RejectedTranslationService(final TaskExecutor taskExecutor,
                                      final Collection<RejectedTranslationPoller> rejectedTranslationPollers) {
        for (final Runnable runnable : rejectedTranslationPollers) {
            taskExecutor.execute(runnable);
        }
    }
}
