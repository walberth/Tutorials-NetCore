namespace EFCore_Demo.Entity
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("direction")]
    public class Direction {
        public int Id { get; set; }
        [Column("person_id")]
        public int IdPerson { get; set; }
        [Column("district_id")]
        public int IdDistrict { get; set; }
        [Column("province_id")]
        public int IdProvince { get; set; }
        [Column("department_id")]
        public int IdDepartment { get; set; }
        [Column("direction")]
        public string DirectionValue { get; set; }

        public virtual Person Person { get; set; }
        public virtual Department Department { get; set; }
        public virtual Province Province { get; set; }
        public virtual District District { get; set; }
    }
}
