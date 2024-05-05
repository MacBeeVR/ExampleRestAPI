using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ExampleRestAPI.Test.APITests;

namespace ExampleRestAPI.Test
{
    [TestClass]
    public class TestDBStateRetainer
    {
        private static string backupFileName;
        private static string backupFileFolder;
        private static string connectionString;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("TestSettings.json")
                .AddUserSecrets<APITestBase>()
                .Build();

            var query               = @"BACKUP DATABASE @db TO DISK = @dbFile";
            connectionString        = config.GetConnectionString("NorthwindDB");
            backupFileFolder        = config.GetValue<string>("DbBackupFolder");
            var sqlConStrBuilder    = new SqlConnectionStringBuilder(connectionString);
            backupFileName          = string.Format($"{backupFileFolder}{sqlConStrBuilder.InitialCatalog}-{DateTime.Now.ToString("yyyy-MM-dd")}.bak");

            if (!File.Exists(backupFileName))
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@dbFile", SqlDbType.NVarChar, 255).Value = backupFileName;
                    command.Parameters.Add("@db", SqlDbType.NVarChar, 128).Value = sqlConStrBuilder.InitialCatalog;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("No Connection String was Set");

            if (string.IsNullOrWhiteSpace(backupFileName))
                throw new Exception("No Database Backup File was Set");

            // TODO: Backup DB
        }
    }
}
