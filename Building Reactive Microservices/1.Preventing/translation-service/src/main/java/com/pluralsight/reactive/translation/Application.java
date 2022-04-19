package com.pluralsight.reactive.translation;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.web.servlet.ServletComponentScan;
import org.springframework.scheduling.annotation.EnableAsync;

@SpringBootApplication
@ServletComponentScan(basePackages = "com.pluralsight.reactive.document")
@EnableAsync
public class Application {

    public static void main(final String[] args) {
        SpringApplication.run(Application.class, args);
    }
}
