package com.pluralsight.reactive.translation.mapper;

import com.pluralsight.reactive.translation.model.domain.Submission;
import com.pluralsight.reactive.translation.model.dto.SubmissionDto;
import java.util.ArrayList;
import java.util.Collection;
import javax.annotation.Generated;
import org.springframework.stereotype.Component;

@Generated(
    value = "org.mapstruct.ap.MappingProcessor",
    date = "2019-12-12T01:14:22-0700",
    comments = "version: 1.3.1.Final, compiler: javac, environment: Java 1.8.0_151 (Oracle Corporation)"
)
@Component
public class SubmissionMapperImpl implements SubmissionMapper {

    @Override
    public SubmissionDto asDto(Submission submission) {
        if ( submission == null ) {
            return null;
        }

        SubmissionDto submissionDto = new SubmissionDto();

        submissionDto.setId( submission.getId() );
        submissionDto.setName( submission.getName() );
        submissionDto.setETag( submission.getETag() );
        submissionDto.setSource( submission.getSource() );
        submissionDto.setTarget( submission.getTarget() );
        submissionDto.setCompletionDate( map( submission.getCompletionDate() ) );
        submissionDto.setStatus( submission.getStatus() );
        submissionDto.setDetails( submission.getDetails() );
        submissionDto.setCreatedAt( map( submission.getCreatedAt() ) );
        submissionDto.setUpdatedAt( map( submission.getUpdatedAt() ) );

        return submissionDto;
    }

    @Override
    public Collection<SubmissionDto> asDto(Collection<Submission> submissions) {
        if ( submissions == null ) {
            return null;
        }

        Collection<SubmissionDto> collection = new ArrayList<SubmissionDto>( submissions.size() );
        for ( Submission submission : submissions ) {
            collection.add( asDto( submission ) );
        }

        return collection;
    }
}
