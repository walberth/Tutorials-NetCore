namespace EFCore_Demo.Model
{
    public class DistrictDto {
        public int Id { get; set; }
        public string Ubigeo { get; set; }
        public string Name { get; set; }
        public ProvinceDto Province { get; set; }
    }
}
