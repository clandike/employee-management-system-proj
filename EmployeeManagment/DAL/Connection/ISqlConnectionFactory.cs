using Microsoft.Data.SqlClient;

namespace DAL.Connection
{
    public interface ISqlConnectionFactory
    {
        SqlConnection CreateConnection();
    }
}
