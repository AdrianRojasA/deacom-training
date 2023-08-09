using DeacomTraining.BusinessClasses;
using DeacomTraining.Data;

namespace DeacomTraining.Service
{
    public class WarehouseService : MainService
    {
        public void InsertOne(Warehouse warehouse)
        {
            SqlExecute.ExecuteCommand($"INSERT INTO tnwrhse(" +
                $"wh_name, wh_addrs, wh_phone, wh_desc) " +
                $"VALUES( '{warehouse.wh_name}', '{warehouse.wh_adrss}', " +
                $"'{warehouse.wh_phone}', '{warehouse.wh_desc}')");
        }

        public void UpdateOne(Warehouse warehouse)
        {
            SqlExecute.ExecuteCommand($"UPDATE tnwrhse(" +
                $"SET wh_name={warehouse.wh_name} " +
                $"wh_adrss={warehouse.wh_adrss} " +
                $"wh_phone={warehouse.wh_phone} " +
                $"wh_desc={warehouse.wh_desc} " +
                $"WHERE wh_id={warehouse.wh_id}");
        }

    }
}
