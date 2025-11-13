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
    }
}
