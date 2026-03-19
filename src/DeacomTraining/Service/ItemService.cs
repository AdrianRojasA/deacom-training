using DeacomTraining.BusinessClasses;
using DeacomTraining.Data;
using DeacomTraining.POCOs;
using System.Data;

namespace DeacomTraining.Service
{
    public class ItemService : MainService
    {
        public IEnumerable<Item> GetAll()
        {
            DataTable dataTable = Cursor.ToCursor("SELECT * FROM tnitem", "ITEMS", connection);
            var items = dataTable.AsEnumerable().Select(item => new Item
            {
                it_id = item.Field<int>("it_id"),
                it_name = item.Field<string>("it_name"),
                it_code = item.Field<string>("it_code"),
                it_desc = item.Field<string>("it_code"),
                it_tpid = item.Field<int>("it_tpid")
            });

            memoryContext.SaveInMemory("CURRENT-ITEMS", dataTable);

            return items;
        }

        public IEnumerable<Item> GetAllByType(int typeId)
        {
            DataTable dataTable = Cursor.ToCursor(
                "SELECT * FROM tnitem " +
                $"WHERE it_tpid=({typeId})",
                "ITEMS", connection);

            var items = dataTable.AsEnumerable().Select(item => new Item
            {
                it_id = item.Field<int>("it_id"),
                it_name = item.Field<string>("it_name"),
                it_code = item.Field<string>("it_code"),
                it_desc = item.Field<string>("it_code"),
                it_tpid = item.Field<int>("it_code")
            });
            return items;
        }

        public void IncreaseQuantity(Entry entry)
        {
            string query = "SELECT * FROM tnitem ";
            string identify = string.Empty;
            string quantityColumn = string.Empty;
            string idColumn = string.Empty;
            string updateTable = string.Empty;

            switch (entry.DestinationType)
            {
                case 0: //-- 0 is a facility
                    query += "JOIN tnitmflty ON tnitem.it_id = tnitmflty.if_itid " +
                        $"WHERE tnitem.it_code = '{entry.ItemCode}'";
                    identify = "if_fcid";
                    quantityColumn = "if_quantity";
                    idColumn = "if_id";
                    updateTable = "tnitmflty";
                    break;
                case 1: //-- 1 is a warehouse
                    query += "JOIN tnitmwhs ON tnitem.it_id = tnitmwhs.iw_itid " +
                        $"WHERE tnitem.it_code = '{entry.ItemCode}'";
                    identify = "iw_whid";
                    quantityColumn = "iw_quantity";
                    idColumn = "iw_id";
                    updateTable = "tnitmwhs";
                    break;
            }

            var response = Cursor.ToCursor(query, "item", connection);

            var filter = response.AsEnumerable().Where(filter =>
                filter.Field<int>(identify) == entry.DestinationId).FirstOrDefault();

            var quantity = filter.Field<decimal>(quantityColumn);
            quantity += entry.AdditionalQuantity;

            SqlExecute.ExecuteCommand($"UPDATE {updateTable} SET {quantityColumn}={quantity} " +
                $"WHERE {updateTable}.{idColumn}={filter.Field<int>(idColumn)}");
        }

    }
}
