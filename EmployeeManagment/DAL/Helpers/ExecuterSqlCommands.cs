using DAL.Connection;
using Microsoft.Data.SqlClient;

namespace DAL.Helpers
{
    internal static class ExecuterSqlCommands
    {
        public static async Task ExecuteNonQuearyAsync(ISqlConnectionFactory connectionFactory, string stringQuery)
        {
            using var connection = connectionFactory.CreateConnection();

            var cmd = new SqlCommand(stringQuery, connection);

            await connection.OpenAsync();

            await cmd.ExecuteNonQueryAsync();
        }

        public static async Task<int?> ExecuteScalarAsync(ISqlConnectionFactory connectionFactory, string stringQuery)
        {
            using var connection = connectionFactory.CreateConnection();

            var cmd = new SqlCommand(stringQuery, connection);

            await connection.OpenAsync();

            var result = await cmd.ExecuteScalarAsync();

            return result is not null ? Convert.ToInt32(result) : throw new InvalidOperationException("Не вдалося отримати Id");
        }
    }
}
