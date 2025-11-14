using DAL.Connection;
using DAL.Helpers;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace DAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ISqlConnectionFactory connectionFactory;

        public EmployeeRepository(ISqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task DeleteAsync(int id)
        {
            var stringQuery = $"DELETE FROM Employee WHERE Id = {id}";

            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery);
        }

        public async Task CreateAsync(Employee entity)
        {
            var stringQuery = $"INSERT INTO Employee (Id, DepartmentId, PositionId, HireDate, Salary)" +
                $" VALUES ({entity.Id}, {entity.DepartmentId}, {entity.PositionId}, '{entity.HireDate.ToString("d")}', {entity.Salary});";

            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            List<Employee> companies = new List<Employee>();

            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM Employee", connection);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                companies.Add(Mapping.MapToEmployee(reader)!);
            }

            return companies;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {

            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM Employee WHERE Id = {id}", connection);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            Employee employee = Mapping.MapToEmployee(reader)!;

            return employee;
        }

        public async Task UpdateAsync(Employee entity)
        {

            var stringQuery = $"UPDATE Employee SET " +
                $"DepartmentId = {entity.DepartmentId}, PositionId = ${entity.PositionId}, HireDate = '{entity.HireDate.ToString("d")}', Salary = {entity.Salary}";

            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery);
        }
    }
}
