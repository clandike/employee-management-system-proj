namespace BAL.DTO
{
    public class EmployeeDTO
    {
        public int Id { get; set; }

        public int DepartmentId { get; set; }

        public int PositionId { get; set; }

        public DateTime HireDate { get; set; }

        public decimal Salary { get; set; }
    }
}
