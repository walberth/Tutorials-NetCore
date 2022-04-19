package com.pluralsight.reactive.translation.mapper;

import com.pluralsight.reactive.translation.model.domain.Submission;
import com.pluralsight.reactive.translation.model.dto.SubmissionDto;
import org.mapstruct.Mapper;
import org.springframework.stereotype.Component;

import java.sql.Timestamp;
import java.util.Collection;

@Component
@Mapper(componentModel = "spring")
public interface SubmissionMapper {

    SubmissionDto asDto(final Submission submission);

    Collection<SubmissionDto> asDto(final Collection<Submission> submissions);

    default Long map(final Timestamp timestamp) {
        return timestamp == null ? null : timestamp.getTime();
    }
}
