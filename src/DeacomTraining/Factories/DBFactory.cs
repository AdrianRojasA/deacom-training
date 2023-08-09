using System.Data.Common;
using DeacomTraining.Data.Connections;
using DeacomTraining.Data.Enums;

namespace DeacomTraining.Data.Factories
{
    public static class DBFactory
    {
        public static DbConnection GetDBConnection(DBTypes dbType)
        {
            switch (dbType)
            {
                case DBTypes.PostgreSQL:
                    return new DBConnectionPG().GetConnection();
                case DBTypes.SQLServer:
                    return new DBConnectionMSS().GetConnection();
                default:
                    throw new Exception("Invalid DB Type");
            }
        }
    }
}