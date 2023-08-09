using System.Data;
using System.Data.Common;
using DeacomTraining.Data.Factories;

namespace DeacomTraining.Data
{
    public sealed class MemoryContext
    {
        private MemoryContext() { }

        private static MemoryContext? _instance;

        private Dictionary<string, DataTable> context = new Dictionary<string, DataTable>();
        private DbConnection _connection = DBFactory.GetDBConnection(Config.Config.DBType);
        public static MemoryContext GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MemoryContext();
            }
            return _instance;
        }

        public Dictionary<string, DataTable> GetMemory()
        {
            return context;
        }

        public DbConnection GetConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            return _connection;
        }

        public void SaveInMemory(string identifier, DataTable data)
        {
            context[identifier] = data;
        }

        public DataTable MakeCopy(string identifier)
        {
            return context[identifier];
        }
    }
}
