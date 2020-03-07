namespace EFCore_Demo.Entity
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("attorney")]
    public class Attorney {
        public int Id { get; set; }
        [Column("person_id")]
        public int IdPerson { get; set; }
        [Column("grade_instruction_type_id")]
        public int IdGradeInstructionType { get; set; }
        [Column("relationship_type_id")]
        public int IdRelationshipType { get; set; }
        [Column("work_center")]
        public string WorkCenter { get; set; } 
        public string Ocupation { get; set; }

        public virtual Person Person { get; set; }
        public virtual Student Student { get; set; }
        public virtual GradeInstructionType GradeInstructionType { get; set; }
        public virtual RelationshipType RelationshipType { get; set; }
    }
}
