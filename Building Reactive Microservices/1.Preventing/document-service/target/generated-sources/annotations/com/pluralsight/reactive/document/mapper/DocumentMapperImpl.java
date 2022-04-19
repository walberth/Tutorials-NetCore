package com.pluralsight.reactive.document.mapper;

import com.pluralsight.reactive.document.model.domain.Document;
import com.pluralsight.reactive.document.model.dto.DocumentDto;
import java.util.ArrayList;
import java.util.Collection;
import javax.annotation.Generated;
import org.springframework.stereotype.Component;

@Generated(
    value = "org.mapstruct.ap.MappingProcessor",
    date = "2019-12-12T01:21:26-0700",
    comments = "version: 1.3.1.Final, compiler: javac, environment: Java 1.8.0_151 (Oracle Corporation)"
)
@Component
public class DocumentMapperImpl implements DocumentMapper {

    @Override
    public DocumentDto asDto(Document document) {
        if ( document == null ) {
            return null;
        }

        DocumentDto documentDto = new DocumentDto();

        documentDto.setName( document.getName() );
        documentDto.setETag( document.getETag() );
        documentDto.setCreatedAt( map( document.getCreatedAt() ) );

        return documentDto;
    }

    @Override
    public Collection<DocumentDto> asDto(Collection<Document> documents) {
        if ( documents == null ) {
            return null;
        }

        Collection<DocumentDto> collection = new ArrayList<DocumentDto>( documents.size() );
        for ( Document document : documents ) {
            collection.add( asDto( document ) );
        }

        return collection;
    }
}
