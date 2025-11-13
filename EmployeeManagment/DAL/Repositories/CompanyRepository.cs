using DAL.Connection;
using DAL.Helpers;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace DAL.Repositories
{
    public class CompanyRepository : IReadableCompanyRepository
    {
        private readonly ISqlConnectionFactory connectionFactory;

        public CompanyRepository(ISqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            List<Company> companies = new List<Company>();

            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM Company", connection);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                companies.Add(Mapping.MapToCompany(reader));
            }

            return companies;
        }

        public async Task<Company> GetByIdAsync(int id)
        {
            Company company;

            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM Company WHERE Id = {id}", connection);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            company = Mapping.MapToCompany(reader);

            return company;
        }
    }
}