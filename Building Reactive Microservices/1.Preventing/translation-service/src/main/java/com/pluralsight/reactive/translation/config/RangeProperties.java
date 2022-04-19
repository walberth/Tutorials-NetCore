package com.pluralsight.reactive.translation.config;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.context.annotation.Profile;

import java.util.ArrayList;
import java.util.Collection;

@Data
@NoArgsConstructor
@AllArgsConstructor
@Profile(value = "m05")
@ConfigurationProperties(prefix = "poller.ranges")
public class RangeProperties {

    private Collection<Range> entries = new ArrayList<>();

    @Data
    @NoArgsConstructor
    @AllArgsConstructor
    public static class Range {
        private String start;
        private String end;
    }
}
