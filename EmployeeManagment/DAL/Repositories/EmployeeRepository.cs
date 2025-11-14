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
            var stringQuery = $"DELETE FROM Employee WHERE Id = @id";
            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery, new {id});
        }

        public async Task CreateAsync(Employee entity)
        {
            var stringQuery = @"
        INSERT INTO Employee (Id, DepartmentId, PositionId, HireDate, Salary)
        VALUES (@Id, @DepartmentId, @PositionId, @HireDate, @Salary);";

            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery, new
            {
                entity.Id,
                entity.DepartmentId,
                entity.PositionId,
                entity.HireDate,
                entity.Salary
            });
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
                companies.Add(DataReaderMappers.MapToEmployee(reader)!);
            }

            return companies;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM Employee WHERE Id = @id", connection);
            cmd.Parameters.AddWithValue("@id", id);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            Employee employee = DataReaderMappers.MapToEmployee(reader)!;

            return employee;
        }

        public async Task UpdateAsync(Employee entity)
        {
            var stringQuery = @"
        UPDATE Employee SET
            DepartmentId = @DepartmentId,
            PositionId = @PositionId,
            HireDate = @HireDate,
            Salary = @Salary
        WHERE Id = @Id;";

            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery, new
            {
                entity.DepartmentId,
                entity.PositionId,
                entity.HireDate,
                entity.Salary,
                entity.Id
            });
        }
    }
}
