namespace EFCore_Demo.Entity
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("person_type")]
    public class PersonType {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Person Person { get; set; }
    }
}
