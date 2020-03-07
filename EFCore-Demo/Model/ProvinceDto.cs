namespace EFCore_Demo.Model
{
    public class ProvinceDto {
        public int Id { get; set; }
        public string Ubigeo { get; set; }
        public string Name { get; set; }
        public DepartmentDto Department { get; set; }
    }
}
