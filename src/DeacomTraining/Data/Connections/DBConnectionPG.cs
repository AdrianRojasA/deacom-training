using System.Data.Common;
using DeacomTraining.Data.Enums;
using DeacomTraining.Data.Interfaces;
using Npgsql;

namespace DeacomTraining.Data.Connections
{
    public class DBConnectionPG : IDBConnection
    {
        //-- change this with your connection string
        private readonly string _connectionString =
            "Host=localhost;Username=training;Password=training;Database=training;";

        public DbConnection GetConnection() => new NpgsqlConnection(_connectionString);

        public void TestConnection()
        {
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                Console.WriteLine("Connection opened successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has ocurred: ${ex.Message}");
            }
        }

        public DBTypes GetDBType()
        {
            return DBTypes.PostgreSQL;
        }
    }
}
