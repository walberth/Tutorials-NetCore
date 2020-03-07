namespace EFCore_Demo.Entity 
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("province")]
    public class Province {
        public int Id { get; set; }
        public string Ubigeo { get; set; }
        public string Name { get; set; }
        [Column("department_id")]
        public int IdDepartment { get; set; }

        public virtual Direction Direction { get; set; }
        public virtual Department Department { get; set; }
        public virtual District District { get; set; }
    }
}
