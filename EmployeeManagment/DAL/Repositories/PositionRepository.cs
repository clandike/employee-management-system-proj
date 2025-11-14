using DAL.Connection;
using DAL.Helpers;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace DAL.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly ISqlConnectionFactory connectionFactory;

        public PositionRepository(ISqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task DeleteAsync(int id)
        {
            var stringQuery = $"DELETE FROM Position WHERE Id = {id}";

            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery);
        }

        public async Task CreateAsync(Position entity)
        {
            var stringQuery = $"INSERT INTO Position (Id, Title, Salary)" +
                $" VALUES ({entity.Id}, {entity.Title}, {entity.Salary});";

            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery);
        }

        public async Task<IEnumerable<Position>> GetAllAsync()
        {
            List<Position> positions = new List<Position>();

            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM Position", connection);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                positions.Add(Mapping.MapToPosition(reader)!);
            }

            return positions;
        }

        public async Task<Position?> GetByIdAsync(int id)
        {

            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM Position WHERE Id = {id}", connection);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            Position position = Mapping.MapToPosition(reader)!;

            return position;
        }

        public async Task UpdateAsync(Position entity)
        {

            var stringQuery = $"UPDATE Position SET Title = {entity.Title}, Salary = ${entity.Salary}";

            await ExecuterSqlCommands.ExecuteNonQuearyAsync(connectionFactory, stringQuery);
        }
    }
}
