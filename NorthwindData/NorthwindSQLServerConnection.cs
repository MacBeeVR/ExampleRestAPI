using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace NorthwindData
{
    public interface INorthwindConnection
    {
        public DbConnection GetConnection();
    }

    public class NorthwindSQLServerConnection : INorthwindConnection
    {
        private readonly string _ConnectionString;

        public NorthwindSQLServerConnection(IConfiguration config)
        {
            _ConnectionString = config.GetConnectionString("NorthwindDB");
            if (string.IsNullOrEmpty(_ConnectionString))
                throw new Exception("Missing NorthwindDB Connection String!");
        }

        public DbConnection GetConnection()
            => new SqlConnection(_ConnectionString);
    }
}
