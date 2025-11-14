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

        public async Task<IEnumerable<Position>> GetAllAsync()
        {
            List<Position> positions = new List<Position>();

            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM Position", connection);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                positions.Add(DataReaderMappers.MapToPosition(reader)!);
            }

            return positions;
        }

        public async Task<Position?> GetByIdAsync(int id)
        {
            using var connection = connectionFactory.CreateConnection();
            var cmd = new SqlCommand($"SELECT * FROM Position WHERE Id = @id", connection);
            cmd.Parameters.AddWithValue("@id", id);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            Position position = DataReaderMappers.MapToPosition(reader)!;

            return position;
        }
    }
}
