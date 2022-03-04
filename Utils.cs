using Microsoft.Data.SqlClient;
using System.Configuration;

namespace SqlToKustoTableGenerator
{
    internal static class Utils
    {
        public static SqlConnection GetSqlConnection()
        {
            string server = Environment.GetEnvironmentVariable("SQL_SERVER") ?? throw new ConfigurationErrorsException("SQL_SERVER environment variable must be set.");
            string username = Environment.GetEnvironmentVariable("SQL_USERNAME") ?? throw new ConfigurationErrorsException("SQL_USERNAME environment variable must be set.");
            string password = Environment.GetEnvironmentVariable("SQL_PASSWORD") ?? throw new ConfigurationErrorsException("SQL_PASSWORD environment variable must be set.");
            string database = Environment.GetEnvironmentVariable("SQL_DATABASE") ?? throw new ConfigurationErrorsException("SQL_DATABASE environment variable must be set.");

            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = server;
            builder.InitialCatalog = database;
            builder.UserID = username;
            builder.Password = password;
            builder.TrustServerCertificate = true;

            var connstring = builder.ToString();

            return GetSqlConnection(connstring);
        }

        public static SqlConnection GetSqlConnection(string connstring)
        {
            var conn = new SqlConnection(connstring);
            conn.Open();
            return conn;
        }
    }
}
