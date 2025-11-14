namespace WebPresentation.Models
{
    public class SalaryReportViewModel
    {
        public int? DepartmentId { get; set; }

        public int? PositionId { get; set; }

        public IEnumerable<SalaryReportItem> Items { get; set; } = new List<SalaryReportItem>();

        public decimal TotalSalary { get; set; }

        public int EmployeeCount { get; set; }
    }
}
