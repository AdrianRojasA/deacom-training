using DeacomTraining.Data;
using System.Data;

namespace DeacomTraining.Service
{
    public class LoginService : MainService
    {
        public bool Load()
        {
            DataTable warehouses = Cursor.ToCursor("SELECT * FROM tnwrhse", "warehouses", connection);
            DataTable facilities = Cursor.ToCursor("SELECT * FROM tnfclty", "facilities", connection);

            memoryContext.SaveInMemory(warehouses.TableName, warehouses);
            memoryContext.SaveInMemory(facilities.TableName, facilities);

            return true;
        }

    }
}
