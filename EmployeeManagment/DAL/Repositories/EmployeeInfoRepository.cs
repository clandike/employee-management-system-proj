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
            var stringQuery = $"DELETE FROM EmployeeInfo WHERE Id = @id";
            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery, new { id });
        }

        public async Task<int?> CreateReturnIdAsync(EmployeeInfo entity)
        {
            var stringQuery = @"
        INSERT INTO EmployeeInfo 
            (FirstName, LastName, MiddleName, Address, Phone, Email, BirthDate)
        VALUES
            (@FirstName, @LastName, @MiddleName, @Address, @Phone, @Email, @BirthDate);
        SELECT SCOPE_IDENTITY();";

            using var connection = connectionFactory.CreateConnection();

            var cmd = new SqlCommand(stringQuery, connection);
            SqlParameterHelper.AddParameters(cmd, new
            {
                entity.FirstName,
                entity.LastName,
                entity.MiddleName,
                entity.Address,
                Phone = entity.PhoneNumber,
                entity.Email,
                entity.BirthDate
            });

            await connection.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();

            return result is not null ? Convert.ToInt32(result) : throw new InvalidOperationException("Не вдалося отримати Id");
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
                companies.Add(DataReaderMappers.MapToEmployeeInfo(reader));
            }

            return companies;
        }

        public async Task<EmployeeInfo?> GetByIdAsync(int id)
        {
            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM EmployeeInfo WHERE Id = @id", connection);
            cmd.Parameters.AddWithValue("@id", id);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            EmployeeInfo employeeInfo = DataReaderMappers.MapToEmployeeInfo(reader);

            return employeeInfo;
        }

        public async Task UpdateAsync(EmployeeInfo entity)
        {
            var stringQuery = @"
        UPDATE EmployeeInfo SET
            FirstName = @FirstName,
            LastName = @LastName,
            MiddleName = @MiddleName,
            Address = @Address,
            Phone = @Phone,
            Email = @Email,
            BirthDate = @BirthDate
        WHERE Id = @Id;";

            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery, new
            {
                entity.FirstName,
                entity.LastName,
                entity.MiddleName,
                entity.Address,
                Phone = entity.PhoneNumber,
                entity.Email,
                entity.BirthDate,
                entity.Id,
            });
        }
    }
}
