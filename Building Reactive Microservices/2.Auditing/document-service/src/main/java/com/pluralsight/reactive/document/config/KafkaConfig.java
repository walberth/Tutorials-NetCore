package com.pluralsight.reactive.document.config;

import com.pluralsight.reactive.document.service.ConsumerLoop;
import com.pluralsight.reactive.document.service.DocumentService;
import com.pluralsight.reactive.document.service.DocumentSubmittedEventConsumerLoop;
import com.pluralsight.reactive.document.service.SubmissionCancelledEventConsumerLoop;
import com.pluralsight.reactive.model.domain.*;
import io.confluent.kafka.serializers.KafkaAvroDeserializer;
import io.confluent.kafka.serializers.KafkaAvroDeserializerConfig;
import io.confluent.kafka.serializers.KafkaAvroSerializer;
import io.confluent.kafka.serializers.KafkaAvroSerializerConfig;
import org.apache.kafka.clients.consumer.ConsumerConfig;
import org.apache.kafka.clients.producer.KafkaProducer;
import org.apache.kafka.clients.producer.ProducerConfig;
import org.apache.kafka.common.serialization.StringDeserializer;
import org.apache.kafka.common.serialization.StringSerializer;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Profile;
import org.springframework.context.annotation.Scope;
import org.springframework.core.task.SimpleAsyncTaskExecutor;
import org.springframework.core.task.TaskExecutor;

import java.util.Properties;
import java.util.UUID;

@Configuration
@Profile(value = "m04 | m05")
public class KafkaConfig {

    @Bean(value = "KafkaProducerProperties")
    @Scope(value = "prototype")
    Properties producerProperties(@Value("${kafka.bootstrap.servers}") final String bootstrapServers,
                                  @Value("${schema.registry.url}") final String schemaRegistryUrl) {
        final Properties properties = new Properties();

        properties.put(ProducerConfig.BOOTSTRAP_SERVERS_CONFIG, bootstrapServers);
        properties.put(ProducerConfig.ACKS_CONFIG, "all");
        properties.put(ProducerConfig.KEY_SERIALIZER_CLASS_CONFIG, StringSerializer.class.getName());
        properties.put(ProducerConfig.VALUE_SERIALIZER_CLASS_CONFIG, KafkaAvroSerializer.class.getName());
        properties.put(KafkaAvroSerializerConfig.SCHEMA_REGISTRY_URL_CONFIG, schemaRegistryUrl);

        return properties;
    }

    @Bean(value = "KafkaConsumerProperties")
    @Scope(value = "prototype")
    Properties consumerProperties(@Value("${kafka.bootstrap.servers}") final String bootstrapServers,
                                  @Value("${schema.registry.url}") final String schemaRegistryUrl) {
        final Properties properties = new Properties();

        properties.put(ConsumerConfig.CLIENT_ID_CONFIG, UUID.randomUUID().toString());
        properties.put(ConsumerConfig.BOOTSTRAP_SERVERS_CONFIG, bootstrapServers);
        properties.put(ConsumerConfig.KEY_DESERIALIZER_CLASS_CONFIG, StringDeserializer.class.getName());
        properties.put(ConsumerConfig.VALUE_DESERIALIZER_CLASS_CONFIG, KafkaAvroDeserializer.class.getName());
        properties.put(KafkaAvroSerializerConfig.SCHEMA_REGISTRY_URL_CONFIG, schemaRegistryUrl);
        properties.put(KafkaAvroDeserializerConfig.SPECIFIC_AVRO_READER_CONFIG, true);

        return properties;
    }

    @Bean(destroyMethod = "close")
    KafkaProducer<String, DocumentLockedEvent> documentLockedEventKafkaProducer(@Qualifier(value = "KafkaProducerProperties") final Properties properties) {
        properties.put(ProducerConfig.CLIENT_ID_CONFIG, UUID.randomUUID().toString());

        return new KafkaProducer<>(properties);
    }

    @Bean(destroyMethod = "close")
    KafkaProducer<String, DocumentUnlockedEvent> documentUnlockedEventKafkaProducer(@Qualifier(value = "KafkaProducerProperties") final Properties properties) {
        properties.put(ProducerConfig.CLIENT_ID_CONFIG, UUID.randomUUID().toString());

        return new KafkaProducer<>(properties);
    }

    @Bean(destroyMethod = "close")
    ConsumerLoop<String, SubmissionCancelledEvent> submissionCancelledEventConsumerLoop(final DocumentService documentService,
                                                                                         @Qualifier(value = "KafkaConsumerProperties") final Properties properties) {
        properties.put(ConsumerConfig.GROUP_ID_CONFIG, "submission-cancelled");

        return new SubmissionCancelledEventConsumerLoop(documentService, "submission-cancelled", properties);
    }

    @Bean(destroyMethod = "close")
    ConsumerLoop<String, DocumentSubmittedEvent> documentSubmittedEventConsumerLoop(final DocumentService documentService,
                                                                                          @Qualifier(value = "KafkaConsumerProperties") final Properties properties) {
        properties.put(ConsumerConfig.GROUP_ID_CONFIG, "document-submitted");

        return new DocumentSubmittedEventConsumerLoop(documentService, "document-submitted", properties);
    }

    @Bean
    public TaskExecutor taskExecutor() {
        return new SimpleAsyncTaskExecutor();
    }
}
