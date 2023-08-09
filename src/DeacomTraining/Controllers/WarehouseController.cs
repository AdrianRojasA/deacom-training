using DeacomTraining.BusinessClasses;
using DeacomTraining.Service;
using Microsoft.AspNetCore.Mvc;

namespace DeacomTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        [HttpPost()]
        public void InsertOne(Warehouse warehouse)
        {
            WarehouseService service = new WarehouseService();
            service.InsertOne(warehouse);
        }

        [HttpPut()]
        public void UpdateOne(Warehouse warehouse)
        {
            WarehouseService service = new WarehouseService();
            service.UpdateOne(warehouse);
        }
    }
}
