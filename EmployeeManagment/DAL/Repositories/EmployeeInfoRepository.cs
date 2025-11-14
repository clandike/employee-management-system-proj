using DAL.Connection;
using DAL.Helpers;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace DAL.Repositories
{
    public class EmployeeInfoRepository : IEmployeeInfoRepository
    {
        private readonly ISqlConnectionFactory connectionFactory;

        public EmployeeInfoRepository(ISqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task DeleteAsync(int id)
        {
            var stringQuery = $"DELETE FROM EmployeeInfo WHERE Id = {id}";

            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery);
        }

        public async Task CreateAsync(EmployeeInfo entity)
        {
            var stringQuery = $"INSERT INTO EmployeeInfo (FirstName, LastName, MiddleName, Address, Phone, Email, BirthDate) VALUES " +
                $"({entity.FirstName}, ${entity.LastName}, {entity.MiddleName}, {entity.Address}, {entity.PhoneNumber}, {entity.Email}, '{entity.BirthDate.ToString("d")}');";

            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery);
        }

        public async Task<IEnumerable<EmployeeInfo>> GetAllAsync()
        {
            List<EmployeeInfo> companies = new List<EmployeeInfo>();

            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM EmployeeInfo", connection);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                companies.Add(Mapping.MapToEmployeeInfo(reader));
            }

            return companies;
        }

        public async Task<EmployeeInfo?> GetByIdAsync(int id)
        {
            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM EmployeeInfo WHERE Id = {id}", connection);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            EmployeeInfo employeeInfo = Mapping.MapToEmployeeInfo(reader);

            return employeeInfo;
        }

        public async Task UpdateAsync(EmployeeInfo entity)
        {
            var stringQuery = $"UPDATE Employee SET " +
                $"FirstName = {entity.FirstName}, LastName = ${entity.LastName}, MiddleName = {entity.MiddleName}, Address = {entity.Address}, Phone = {entity.PhoneNumber}, Email = {entity.Email}, BirthDate = '{entity.BirthDate.ToString("d")}'";

            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery);
        }
    }
}
