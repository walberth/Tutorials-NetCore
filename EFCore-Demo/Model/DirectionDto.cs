namespace EFCore_Demo.Model
{
    public class DirectionDto {
        public int Id { get; set; }
        public DistrictDto District { get; set; }
        public ProvinceDto Province { get; set; }
        public DepartmentDto Department { get; set; }
        public string Direction { get; set; }
    }
}
