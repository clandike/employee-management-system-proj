using Microsoft.Data.SqlClient;

namespace DAL.Helpers
{
    public static class SqlDataReaderExtensions
    {
        public static T GetFieldValueSafe<T>(this SqlDataReader reader, string column)
        {
            var index = reader.GetOrdinal(column);
            if (reader.IsDBNull(index))
                return default!;
            return (T)reader.GetValue(index);
        }
    }
}
