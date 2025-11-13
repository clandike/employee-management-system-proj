using DAL.Models;
using Microsoft.Data.SqlClient;

namespace DAL.Helpers
{
    public static class Mapping
    {
        public static Company MapToCompany(this SqlDataReader reader)
        {
            return new Company
            {
                Id = reader.GetFieldValueSafe<int>("Id"),
                Name = reader.GetFieldValueSafe<string>("Name"),
                Address = reader.GetFieldValueSafe<string>("Address"),
                PhoneNumber = reader.GetFieldValueSafe<string>("Phone"),
                Email = reader.GetFieldValueSafe<string>("Email"),
                Description = reader.GetFieldValueSafe<string>("Description")
            };
        }

        public static Employee MapToEmployee(this SqlDataReader reader)
        {

            return new Employee
            {
                Id = reader.GetFieldValueSafe<int>("Id"),
                DepartmentId = reader.GetFieldValueSafe<int>("DepartmentId"),
                PositionId = reader.GetFieldValueSafe<int>("PositionId"),
                HireDate = reader.GetFieldValueSafe<DateTime>("HireDate"),
                Salary = reader.GetFieldValueSafe<decimal>("Salary")
            };
        }

        public static EmployeeInfo MapToEmployeeInfo(this SqlDataReader reader)
        {

            return new EmployeeInfo
            {
                Id = reader.GetFieldValueSafe<int>("Id"),
                FirstName = reader.GetFieldValueSafe<string>("FirstName"),
                LastName = reader.GetFieldValueSafe<string>("LastName"),
                MiddleName = reader.GetFieldValueSafe<string?>("MiddleName"),
                Address = reader.GetFieldValueSafe<string>("Address"),
                PhoneNumber = reader.GetFieldValueSafe<string>("PhoneNumber"),
                Email = reader.GetFieldValueSafe<string>("Email"),
                BirthDate = reader.GetFieldValueSafe<DateTime>("BirthDate")
            };
        }
    }
}
