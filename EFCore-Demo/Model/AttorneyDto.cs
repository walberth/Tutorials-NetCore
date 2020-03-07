namespace EFCore_Demo.Model
{
    public class AttorneyDto {
        public int Id { get; set; }
        public PersonDto Person { get; set; }
        public GradeInstructionTypeDto GradeInstructionType { get; set; }
        public RelationshipTypeDto RelationshipType { get; set; }
        public string WorkCenter { get; set; }
        public string Ocupation { get; set; }
    }
}
