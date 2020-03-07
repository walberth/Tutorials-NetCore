namespace EFCore_Demo.Entity
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("student")]
    public class Student {
        public int Id { get; set; }
        [Column("person_id")]
        public int IdPerson { get; set; }
        [Column("attorney_id")]
        public int IdAttorney { get; set; }
        [Column("person_live_id")]
        public int IdPersonLive { get; set; }
        [Column("study_certificate")]
        public bool StudyCertificate { get; set; }
        [Column("brother_number")]
        public int BrotherNumber { get; set; }
        [Column("reference_type_id")]
        public int IdReferenceType { get; set; }

        public virtual ReferenceType ReferenceType { get; set; }
        public virtual Person Person { get; set; }
        public virtual Attorney Attorney { get; set; }
    }
}
