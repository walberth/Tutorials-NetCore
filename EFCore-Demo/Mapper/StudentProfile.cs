namespace EFCore_Demo.Mapper 
{
    using Model;
    using Entity;

    public class StudentProfile : AutoMapper.Profile {
        public StudentProfile() {
            CreateMap<Student, StudentDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.Person))
                    .ForMember(dest => dest.Attorney, opt => opt.MapFrom(src => src.Attorney))
                    .ForMember(dest => dest.ReferenceType, opt => opt.MapFrom(src => src.ReferenceType))
                    .ForMember(dest => dest.IdPersonLive, opt => opt.MapFrom(src => src.IdPersonLive))
                    .ForMember(dest => dest.StudyCertificate, opt => opt.MapFrom(src => src.StudyCertificate))
                    .ForMember(dest => dest.BrotherNumber, opt => opt.MapFrom(src => src.BrotherNumber))
                    .ReverseMap();

            CreateMap<RelationshipType, RelationshipTypeDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ReverseMap();

            CreateMap<ReferenceType, ReferenceTypeDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ReverseMap();

            CreateMap<Province, ProvinceDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Ubigeo, opt => opt.MapFrom(src => src.Ubigeo))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
                    .ReverseMap();

            CreateMap<PersonType, PersonTypeDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ReverseMap();

            CreateMap<Person, PersonDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                    .ForMember(dest => dest.FatherName, opt => opt.MapFrom(src => src.FatherName))
                    .ForMember(dest => dest.MotherName, opt => opt.MapFrom(src => src.MotherName))
                    .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.Sex))
                    .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                    .ForMember(dest => dest.PersonType, opt => opt.MapFrom(src => src.PersonType))
                    .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.DocumentType))
                    .ForMember(dest => dest.Direction, opt => opt.MapFrom(src => src.Direction))
                    .ForMember(dest => dest.Document, opt => opt.MapFrom(src => src.Document))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                    .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Mobile))
                    .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.Telephone))
                    .ReverseMap();

            CreateMap<GradeInstructionType, GradeInstructionTypeDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ReverseMap();

            CreateMap<DocumentType, DocumentTypeDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ReverseMap();

            CreateMap<District, DistrictDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Ubigeo, opt => opt.MapFrom(src => src.Ubigeo))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Province))
                    .ReverseMap();

            CreateMap<Direction, DirectionDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District))
                    .ForMember(dest => dest.Province, opt => opt.MapFrom(src => src.Province))
                    .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
                    .ForMember(dest => dest.Direction, opt => opt.MapFrom(src => src.DirectionValue))
                    .ReverseMap();

            CreateMap<Department, DepartmentDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Ubigeo, opt => opt.MapFrom(src => src.Ubigeo))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ReverseMap();

            CreateMap<Attorney, AttorneyDto>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.Person))
                    .ForMember(dest => dest.GradeInstructionType, opt => opt.MapFrom(src => src.GradeInstructionType))
                    .ForMember(dest => dest.RelationshipType, opt => opt.MapFrom(src => src.RelationshipType))
                    .ForMember(dest => dest.WorkCenter, opt => opt.MapFrom(src => src.WorkCenter))
                    .ForMember(dest => dest.Ocupation, opt => opt.MapFrom(src => src.Ocupation))
                    .ReverseMap();
        }
    }
}
