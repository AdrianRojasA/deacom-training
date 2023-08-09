using System.Data.Common;

namespace DeacomTraining.Data
{
    public static class SqlExecute
    {
        public static void ExecuteCommand(string sqlCommand)
        {
            DbConnection db = MemoryContext.GetInstance().GetConnection();

            try
            {
                var cmd = db.CreateCommand();
                cmd.CommandText = sqlCommand;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
