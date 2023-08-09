using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using DeacomTraining.Data.Enums;
using DeacomTraining.Data.Interfaces;

namespace DeacomTraining.Data.Connections
{
    public class DBConnectionMSS : IDBConnection
    {

        //-- change this with your connection string
        private readonly string _connectionString =
            "Data Source=RALARCONV-NH01;Initial Catalog=DeacomTraining;Trusted_Connection=True;";

        public DbConnection GetConnection() => new SqlConnection(_connectionString);

        public void TestConnection()
        {
            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();
                Console.WriteLine("Successful connection ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has ocurred: ${ex.Message}");
            }
        }

        public DBTypes GetDBType()
        {
            return DBTypes.SQLServer;
        }
    }
}
