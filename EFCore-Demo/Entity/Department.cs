namespace EFCore_Demo.Entity
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("department")]
    public class Department {
        public int Id { get; set; }
        public string Ubigeo { get; set; }
        public string Name { get; set; }

        public virtual Direction Direction { get; set; }
        public virtual Province Province { get; set; }
    }
}
