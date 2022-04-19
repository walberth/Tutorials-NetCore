package com.pluralsight.reactive.document.config;

import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.boot.jdbc.DataSourceBuilder;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Primary;
import org.springframework.jdbc.core.JdbcTemplate;

import javax.sql.DataSource;

@Configuration
public class DataSourceConfig {

    @Primary
    @Bean(value = "ReadDataSource")
    @ConfigurationProperties(prefix = "spring.read.datasource")
    DataSource readDataSource() {
        return DataSourceBuilder.create().build();
    }

    @Bean(value = "WriteDataSource")
    @ConfigurationProperties(prefix = "spring.read.datasource")
    DataSource writeDataSource() {
        return DataSourceBuilder.create().build();
    }

    @Bean(value = "ReadJdbcTemplate")
    JdbcTemplate readJdbcTemplate(@Qualifier(value = "ReadDataSource") final DataSource dataSource) {
        return new JdbcTemplate(dataSource);
    }

    @Bean(value = "WriteJdbcTemplate")
    JdbcTemplate writeJdbcTemplate(@Qualifier(value = "WriteDataSource") final DataSource dataSource) {
        return new JdbcTemplate(dataSource);
    }
}
