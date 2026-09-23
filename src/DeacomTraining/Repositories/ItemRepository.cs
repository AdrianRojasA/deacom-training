using DeacomTraining.BusinessClasses;
using DeacomTraining.Data;

namespace DeacomTraining.Repositories
{
    public class ItemRepository
    {
        private IDBConnection _connection;

        public ItemRepository()
        {
            _connection = DBFactory.GetConnection();
        }

        public List<Item> GetAll()
        {
            var items = new List<Item>();
            var dt = _connection.ExecuteCommand("SELECT * FROM tnitem");
            
            foreach (DataRow row in dt.Rows)
            {
                items.Add(MapDataRowToItem(row));
            }
            
            return items;
        }

        public Item GetById(int id)
        {
            var dt = _connection.ExecuteCommand($"SELECT * FROM tnitem WHERE item_id = {id}");
            
            if (dt.Rows.Count == 0)
                return null;
                
            return MapDataRowToItem(dt.Rows[0]);
        }

        public void Insert(Item item)
        {
            var conn = new DBConnection();
            conn.ExecuteCommand($"INSERT INTO tnitem (item_name, item_desc) VALUES ('{item.item_name}', '{item.item_desc}')");
        }

        private Item MapDataRowToItem(DataRow row)
        {
            return new Item
            {
                item_id = (int)row["item_id"],
                item_name = (string)row["item_name"],
                item_desc = (string)row["item_desc"]
            };
        }
    }
}
