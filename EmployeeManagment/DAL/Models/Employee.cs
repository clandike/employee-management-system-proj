namespace DAL.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public int DepartmentId { get; set; }

        public int PositionId { get; set; }

        public DateTime HireDate { get; set; }

        public decimal Salary { get; set; }
    }
}
