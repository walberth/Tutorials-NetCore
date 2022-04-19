package com.pluralsight.reactive.translation.service;

import lombok.extern.slf4j.Slf4j;
import org.apache.kafka.clients.consumer.ConsumerRecord;
import org.apache.kafka.clients.consumer.ConsumerRecords;
import org.apache.kafka.clients.consumer.KafkaConsumer;

import java.time.Duration;
import java.time.temporal.ChronoUnit;
import java.util.Collections;
import java.util.Properties;

@Slf4j
public abstract class ConsumerLoop<K, V> implements Runnable {

    private final KafkaConsumer<K, V> consumer;
    private final String topic;

    public ConsumerLoop(final String topic, final Properties properties) {
        this.topic = topic;
        this.consumer = new KafkaConsumer<>(properties);
    }

    @Override
    public void run() {
        consumer.subscribe(Collections.singletonList(topic));

        while (true) {
            try {
                final ConsumerRecords<K, V> records =
                        consumer.poll(Duration.of(10000, ChronoUnit.MILLIS));

                log.info("Found {} records.", records.count());

                for (final ConsumerRecord<K, V> record : records) {
                    log.info("Consuming record, {}.", record);

                    handleRecord(record);
                    consumer.commitSync();
                }
            } catch (final RuntimeException e) {
                log.error("Caught exception while processing records.", e);
            }
        }
    }

    public void close() {
        consumer.close();
    }

    public abstract void handleRecord(final ConsumerRecord<K, V> consumerRecord);
}
