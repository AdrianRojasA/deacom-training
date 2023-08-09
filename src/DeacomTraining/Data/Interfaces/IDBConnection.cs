using System.Data.Common;
using DeacomTraining.Data.Enums;

namespace DeacomTraining.Data.Interfaces
{
    public interface IDBConnection
    {
        DbConnection GetConnection();
        void TestConnection();
        DBTypes GetDBType();
    }
}