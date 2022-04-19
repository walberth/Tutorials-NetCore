package com.pluralsight.reactive.translation.config;

import com.pluralsight.reactive.model.domain.DocumentSubmittedEvent;
import com.pluralsight.reactive.translation.service.MissedNotificationPoller;
import com.pluralsight.reactive.translation.service.RejectedTranslationPoller;
import com.pluralsight.reactive.translation.service.SubmissionService;
import org.apache.kafka.clients.producer.KafkaProducer;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Profile;
import org.springframework.jdbc.core.JdbcTemplate;

import java.net.InetAddress;
import java.net.UnknownHostException;
import java.util.ArrayList;
import java.util.Collection;

@Configuration
@Profile(value = "m05")
public class PollerConfig {

    @Bean(name = "Hostname")
    String hostname() {
        try {
            return InetAddress.getLocalHost().getHostName();
        } catch (final UnknownHostException e) {
            throw new RuntimeException("Unable to determine hostname.");
        }
    }

    @Bean
    Collection<MissedNotificationPoller> missedNotificationsPollers(final RangeProperties rangeProperties,
                                                                    @Value(value = "${poller.missed.notification.threshold}") final long threshold,
                                                                    @Value(value = "${poller.missed.notification.limit}") final int limit,
                                                                    @Qualifier(value = "Hostname") final String hostname,
                                                                    final KafkaProducer<String, DocumentSubmittedEvent> documentSubmittedEventKafkaProducer,
                                                                    final SubmissionService submissionService,
                                                                    @Qualifier(value = "ReadJdbcTemplate") final JdbcTemplate readJdbcTemplate,
                                                                    @Qualifier(value = "WriteJdbcTemplate") final JdbcTemplate writeJdbcTemplate) {
        final Collection<MissedNotificationPoller> missedNotificationPollers = new ArrayList<>();

        for (final RangeProperties.Range range : rangeProperties.getEntries()) {
            final MissedNotificationPoller missedNotificationPoller =
                    new MissedNotificationPoller(range.getStart(),
                                                 range.getEnd(),
                                                 threshold,
                                                 limit,
                                                 hostname,
                                                 documentSubmittedEventKafkaProducer,
                                                 submissionService,
                                                 readJdbcTemplate,
                                                 writeJdbcTemplate);

            missedNotificationPollers.add(missedNotificationPoller);
        }

        return missedNotificationPollers;
    }

    @Bean
    Collection<RejectedTranslationPoller> rejectedTranslationPollers(final RangeProperties rangeProperties,
                                                                     @Value(value = "${poller.rejected.translation.threshold}") final long threshold,
                                                                     @Value(value = "${poller.rejected.translation.limit}") final int limit,
                                                                     @Qualifier(value = "Hostname") final String hostname,
                                                                     final SubmissionService submissionService,
                                                                     @Qualifier(value = "ReadJdbcTemplate") final JdbcTemplate readJdbcTemplate,
                                                                     @Qualifier(value = "WriteJdbcTemplate") final JdbcTemplate writeJdbcTemplate) {
        final Collection<RejectedTranslationPoller> rejectedTranslationPollers = new ArrayList<>();

        for (final RangeProperties.Range range : rangeProperties.getEntries()) {
            final RejectedTranslationPoller rejectedTranslationPoller =
                    new RejectedTranslationPoller(range.getStart(),
                                                  range.getEnd(),
                                                  threshold,
                                                  limit,
                                                  hostname,
                                                  submissionService,
                                                  readJdbcTemplate,
                                                  writeJdbcTemplate);

            rejectedTranslationPollers.add(rejectedTranslationPoller);
        }

        return rejectedTranslationPollers;
    }
}
