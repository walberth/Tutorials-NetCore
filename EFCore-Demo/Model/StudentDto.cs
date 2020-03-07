namespace EFCore_Demo.Model
{
    public class StudentDto {
        public int Id { get; set; }
        public PersonDto Person { get; set; }
        public AttorneyDto Attorney { get; set; }
        public ReferenceTypeDto ReferenceType { get; set; }
        public int IdPersonLive { get; set; }
        public bool StudyCertificate { get; set; }
        public int BrotherNumber { get; set; }
    }
}
