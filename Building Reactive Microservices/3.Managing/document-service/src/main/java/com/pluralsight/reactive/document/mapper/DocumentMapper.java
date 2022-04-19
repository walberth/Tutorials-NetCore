package com.pluralsight.reactive.document.mapper;

import com.pluralsight.reactive.document.model.domain.Document;
import com.pluralsight.reactive.document.model.dto.DocumentDto;
import org.mapstruct.Mapper;
import org.springframework.stereotype.Component;

import java.sql.Timestamp;
import java.util.Collection;

@Component
@Mapper(componentModel = "spring")
public interface DocumentMapper {

    DocumentDto asDto(final Document document);

    Collection<DocumentDto> asDto(final Collection<Document> documents);

    default Long map(final Timestamp timestamp) {
        return timestamp == null ? null : timestamp.getTime();
    }
}
