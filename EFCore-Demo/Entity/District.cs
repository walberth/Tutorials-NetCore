namespace EFCore_Demo.Entity
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("district")]
    public class District {
        public int Id { get; set; }
        public string Ubigeo { get; set; }
        public string Name { get; set; }
        [Column("province_id")]
        public int IdProvince { get; set; }

        public virtual Direction Direction { get; set; }
        public virtual Province Province { get; set; }
    }
}
