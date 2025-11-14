namespace WebPresentation.Models
{
    public class EmployeeFilterDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public string Search { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Sort { get; set; } = "name";
    }
}
