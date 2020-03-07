namespace EFCore_Demo.Model
{
    using System;

    public class PersonDto {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public bool Sex { get; set; }
        public DateTime BirthDate { get; set; }
        public PersonTypeDto PersonType { get; set; }
        public DocumentTypeDto DocumentType { get; set; }
        public DirectionDto Direction { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }
    }
}
