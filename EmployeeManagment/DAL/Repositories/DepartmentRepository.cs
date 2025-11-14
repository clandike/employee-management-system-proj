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

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            List<Department> departments = new List<Department>();

            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM Department", connection);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                departments.Add(DataReaderMappers.MapToDepartment(reader)!);
            }

            return departments;
        }

        public async Task<Department?> GetByIdAsync(int id)
        {

            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM Department WHERE Id = @id", connection);
            cmd.Parameters.AddWithValue("@id", id);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            Department department = DataReaderMappers.MapToDepartment(reader)!;

            return department;
        }
    }
}
