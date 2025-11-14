using DAL.Connection;
using DAL.Helpers;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace DAL.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ISqlConnectionFactory connectionFactory;

        public DepartmentRepository(ISqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task DeleteAsync(int id)
        {
            var stringQuery = $"DELETE FROM Department WHERE Id = {id}";

            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery);
        }

        public async Task CreateAsync(Department entity)
        {
            var stringQuery = $"INSERT INTO Department (Id, Name, CompanyId)" +
                $" VALUES ({entity.Id}, {entity.Name}, {entity.CompanyId});";

            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery);
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            List<Department> departments = new List<Department>();

            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM Department", connection);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                departments.Add(Mapping.MapToDepartment(reader)!);
            }

            return departments;
        }

        public async Task<Department?> GetByIdAsync(int id)
        {

            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM Department WHERE Id = {id}", connection);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            Department department = Mapping.MapToDepartment(reader)!;

            return department;
        }

        public async Task UpdateAsync(Department entity)
        {

            var stringQuery = $"UPDATE Department SET Name = '{entity.Name}', CompanyId = {entity.CompanyId} WHERE Id = {entity.Id}";

            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery);
        }
    }
}
