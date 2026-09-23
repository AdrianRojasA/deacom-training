using DeacomTraining.BusinessClasses;
using DeacomTraining.Service;
using DeacomTraining.Data;
using Microsoft.AspNetCore.Mvc;

namespace DeacomTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        [HttpGet]
        public List<Item> GetAllItems()
        {
            var items = new List<Item>();
            var connection = DBFactory.GetConnection();
            var dtItems = connection.ExecuteCommand($"SELECT * FROM tnitem");
            
            if (dtItems != null && dtItems.Rows.Count > 0)
            {
                foreach (DataRow dr in dtItems.Rows)
                {
                    items.Add(new Item 
                    { 
                        item_id = (int)dr["item_id"],
                        item_name = (string)dr["item_name"],
                        item_desc = (string)dr["item_desc"]
                    });
                }
            }
            return items;
        }

        [HttpPost]
        public void CreateItem(Item lcItem)
        {
            SqlExecute.ExecuteCommand($"INSERT INTO tnitem (item_name, item_desc) " +
                $"VALUES ('{lcItem.item_name}', '{lcItem.item_desc}')");
        }

        [HttpGet("{id}")]
        public Item GetItemById(int id)
        {
            var connection = DBFactory.GetConnection();
            var dtItem = connection.ExecuteCommand($"SELECT * FROM tnitem WHERE item_id = {id}");
            
            if (dtItem.Rows.Count > 0)
            {
                var dr = dtItem.Rows[0];
                return new Item 
                { 
                    item_id = (int)dr["item_id"],
                    item_name = (string)dr["item_name"],
                    item_desc = (string)dr["item_desc"]
                };
            }
            
            return null;
        }
    }
}
