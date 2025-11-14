using DAL.Connection;
using Microsoft.Data.SqlClient;

namespace DAL.Helpers
{
    internal static class ExecuterSqlCommands
    {
        public static async Task ExecuteNonQuearyAsync(ISqlConnectionFactory connectionFactory, string stringQuery, object parameters)
        {
            using var connection = connectionFactory.CreateConnection();

            var cmd = new SqlCommand(stringQuery, connection);

            SqlParameterHelper.AddParameters(cmd, parameters);

            await connection.OpenAsync();

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
