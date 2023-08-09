using DeacomTraining.BusinessClasses;
using DeacomTraining.POCOs;
using DeacomTraining.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeacomTraining.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        [HttpGet()]
        public IEnumerable<Item> GetAll()
        {
            ItemService service = new ItemService();
            return service.GetAll();
        }

        [HttpGet("TypeId/{typeId}")]
        public IEnumerable<Item> GetAllByType(int typeId)
        {
            ItemService service = new ItemService();
            return service.GetAllByType(typeId);
        }

        [HttpPost("Entry")]
        public void IncreaseQuantity(Entry entry)
        {
            ItemService service = new ItemService();
            service.IncreaseQuantity(entry);
        }
    }
}
