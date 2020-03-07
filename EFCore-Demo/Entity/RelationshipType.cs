namespace EFCore_Demo.Entity
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("relationship_type")]
    public class RelationshipType {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Attorney Attorney { get; set; }
    }
}
