namespace WebPresentation.Models
{
    public class SalaryReportItem
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Department { get; set; } = null!;

        public string Position { get; set; } = null!;

        public decimal Salary { get; set; }

        public int DepartmentId { get; set; }

        public int PositionId { get; set; }
    }
}
