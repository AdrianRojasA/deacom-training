using DeacomTraining.BusinessClasses;
using DeacomTraining.Data;

namespace DeacomTraining.Service
{
    public class ItemService : MainService
    {
        public List<Item> GetAllWithType(int type_id)
        {
            var connection = DBFactory.GetConnection();
            var results = new List<Item>();
            
            var query = $"SELECT tnitem.*, tntype.type_name FROM tnitem " +
                        $"INNER JOIN tntype ON tnitem.type_id = tntype.type_id " +
                        $"WHERE tnitem.type_id = {type_id}";
            
            var dt = connection.ExecuteCommand(query);
            
            foreach (DataRow row in dt.Rows)
            {
                results.Add(new Item 
                { 
                    item_id = (int)row["item_id"],
                    item_name = (string)row["item_name"],
                    desc = (string)row["item_desc"], 
                    type_id = type_id
                });
            }
            
            return results;
        }

        public void InsertItem(Item lcoItem)
        {
            SqlExecute.ExecuteCommand($"INSERT INTO tnitem (item_name, item_desc, type_id) " +
                $"VALUES ('{lcoItem.item_name}', '{lcoItem.item_desc}', {lcoItem.type_id})");
        }

        public void BulkInsertItems(List<Item> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                SqlExecute.ExecuteCommand($"INSERT INTO tnitem (item_name, item_desc, type_id) " +
                    $"VALUES ('{item.item_name}', '{item.item_desc}', {item.type_id})");
            }
        }
    }
}
