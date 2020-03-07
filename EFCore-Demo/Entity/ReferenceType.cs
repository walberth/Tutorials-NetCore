namespace EFCore_Demo.Entity
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("reference_type")]
    public class ReferenceType {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Student Student { get; set; }
    }
}
