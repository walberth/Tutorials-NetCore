namespace EFCore_Demo.Entity
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("grade_instruction_type")]
    public class GradeInstructionType {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Attorney Attorney { get; set; }
    }
}
