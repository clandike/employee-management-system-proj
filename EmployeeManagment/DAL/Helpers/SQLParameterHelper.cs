using Microsoft.Data.SqlClient;

namespace DAL.Helpers
{
    public static class SqlParameterHelper
    {
        public static void AddParameters(SqlCommand cmd, object parameters)
        {
            var props = parameters.GetType().GetProperties();
            foreach (var prop in props)
            {
                var value = prop.GetValue(parameters) ?? DBNull.Value;
                cmd.Parameters.AddWithValue("@" + prop.Name, value);
            }
        }
    }
}
