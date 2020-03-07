namespace EFCore_Demo.Entity 
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("person")]
    public class Person {
        public int Id { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; }
        [Column("father_name")]
        public string FatherName { get; set; }
        [Column("mother_name")]
        public string MotherName { get; set; }
        public bool Sex { get; set; }
        [Column("birth_date")]
        public DateTime BirthDate { get; set; }
        [Column("person_type_id")]
        public int IdPersonType { get; set; }
        [Column("document_type_id")]
        public int IdDocumentType { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Telephone { get; set; }

        public virtual DocumentType DocumentType { get; set; }
        public virtual PersonType PersonType { get; set; }
        public virtual Attorney Attorney { get; set; }
        public virtual Direction Direction { get; set; }
        public virtual Student Student { get; set; }
    }
}
