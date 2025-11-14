namespace WebPresentation.Models
{
    public class EmployeeFullViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? MiddleName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string BirthDate { get; set; }

        public string Department { get; set; }

        public string Position { get; set; }

        public string HireDate { get; set; }

        public decimal Salary { get; set; }

        public int DepartmentId { get; set; }

        public int PositionId { get; set; }
    }
}
