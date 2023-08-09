using System.Data;
using System.Data.Common;

namespace DeacomTraining.Data
{
    public static class Cursor
    {
        public static DataTable ToCursor(string sqlCommand, string tableName, DbConnection db)
        {
            DataTable dataReturn = new DataTable(tableName);

            var cmd = db.CreateCommand();
            cmd.CommandText = sqlCommand;
            try
            {
                var reader = cmd.ExecuteReader();
                dataReturn.Load(reader);
                return dataReturn;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad: " + ex.Message);
                throw;
            }
        }

    }
}
