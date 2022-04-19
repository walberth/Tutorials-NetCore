package com.pluralsight.reactive.document.config;

import com.fasterxml.jackson.annotation.JsonInclude.Include;
import com.fasterxml.jackson.databind.DeserializationFeature;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.SerializationFeature;
import com.fasterxml.jackson.databind.annotation.JsonPOJOBuilder;
import com.fasterxml.jackson.databind.introspect.AnnotatedClass;
import com.fasterxml.jackson.databind.introspect.JacksonAnnotationIntrospector;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class JacksonConfig {

    @Bean
    JacksonAnnotationIntrospector jacksonAnnotationIntrospector() {
        return new JacksonAnnotationIntrospector() {

            @Override
            public JsonPOJOBuilder.Value findPOJOBuilderConfig(final AnnotatedClass annotatedClass) {
                if (annotatedClass.hasAnnotation(JsonPOJOBuilder.class)) {
                    return super.findPOJOBuilderConfig(annotatedClass);
                }
                return new JsonPOJOBuilder.Value("build", "");
            }
        };
    }

    @Bean
    ObjectMapper objectMapper(final JacksonAnnotationIntrospector jacksonAnnotationIntrospector) {
        return new ObjectMapper()
                .setDefaultPropertyInclusion(Include.NON_NULL)
                .disable(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES)
                .disable(SerializationFeature.WRITE_DATES_AS_TIMESTAMPS)
                .setAnnotationIntrospector(jacksonAnnotationIntrospector)
                .findAndRegisterModules();
    }
}

