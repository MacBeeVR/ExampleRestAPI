using System.Data;
using Microsoft.Data.SqlClient;
using ExampleRestAPI.Test.APITests;
using Microsoft.Extensions.Configuration;

namespace ExampleRestAPI.Test
{
    /// <summary>
    /// Holds Methods that Execute before and After Tests
    /// That Creates a Backup of the Test DB before Tests
    /// and Restores from the Backup After the Tests
    /// </summary>
    [TestClass]
    public class TestDBStateRetainer
    {
        #region DB Restore Fields
        private static string masterDBName      = string.Empty;
        private static string databaseName      = string.Empty;
        private static string backupFilePath    = string.Empty;
        private static string connectionString  = string.Empty;
        #endregion

        /// <summary>
        /// Backup Test Database before tests are ran.
        /// </summary>
        /// <param name="context"></param>
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            // Build Configuration Using TestSettings and Project Secrets for Config Based Backup / Restore
            var config = new ConfigurationBuilder()
                .AddJsonFile("TestSettings.json")
                .AddUserSecrets<APITestBase>()
                .Build();

            masterDBName            = config.GetValue<string>("MasterDBName");
            connectionString        = config.GetConnectionString("NorthwindDB");
            var backupFileFolder    = config.GetValue<string>("DbBackupFolder");
            var sqlConStrBuilder    = new SqlConnectionStringBuilder(connectionString);
            databaseName            = sqlConStrBuilder.InitialCatalog;
            backupFilePath          = string.Format($"{backupFileFolder}{databaseName}-{DateTime.Now.ToString("yyyy-MM-dd")}.bak");

            // If the Backup File Doesn't Exist Already, Create It.
            if (!File.Exists(backupFilePath))
            {
                // Remove Old Backup Files before Creating New One.
                // This Ensures the Backup to Restore is Current to the Current Day
                foreach(var file in Directory.GetFiles(backupFilePath))
                    File.Delete(file);

                // Backup the Database
                var backupQry = @"BACKUP DATABASE @db TO DISK = @dbFile";
                using (var connection   = new SqlConnection(connectionString))
                using (var command      = new SqlCommand(backupQry, connection))
                {
                    command.Parameters.Add("@db"    , SqlDbType.NVarChar, 128).Value = databaseName;
                    command.Parameters.Add("@dbFile", SqlDbType.NVarChar, 255).Value = backupFilePath;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Restore Test Database after all tests are complete
        /// </summary>
        /// <exception cref="Exception"></exception>
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            #region Ensure Required Restore Fields
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("No Connection String was Set");
            if (string.IsNullOrWhiteSpace(backupFilePath) || !File.Exists(backupFilePath))
                throw new Exception("No Database Backup File was Set");
            if (string.IsNullOrWhiteSpace(databaseName))
                throw new Exception("No Database Name was Set");
            if (string.IsNullOrWhiteSpace(masterDBName))
                throw new Exception("No Master Database Name was Set");
            #endregion

            var useMasterQry = $"USE [{masterDBName}]";
            var restoreDBQry = $@"RESTORE DATABASE [{databaseName}] FROM DISK = @backupFile";

            // Open Connection, Switch to Master DB, then Restore the Test DB
            using (var connection   = new SqlConnection(connectionString))
            using (var masterCMD    = new SqlCommand(useMasterQry, connection))
            using (var restoreCMD   = new SqlCommand(restoreDBQry  , connection))
            {
                connection.Open();
                masterCMD.ExecuteNonQuery();
                restoreCMD.Parameters.Add("@backupFile", SqlDbType.NVarChar).Value = backupFilePath;
                restoreCMD.ExecuteNonQuery();
            }
        }
    }
}
