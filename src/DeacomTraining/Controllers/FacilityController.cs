using DeacomTraining.BusinessClasses;
using DeacomTraining.Service;
using Microsoft.AspNetCore.Mvc;

namespace DeacomTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilityController : ControllerBase
    {
        [HttpGet("Id/{id}")]
        public Facility GetFacility(int id)
        {
            FacilityService service = new FacilityService();

            Facility facility = service.GetOne(id);
            return facility;

        }

        [HttpGet()]
        public IEnumerable<Facility> GetAllFacilities()
        {
            FacilityService service = new FacilityService();

            IEnumerable<Facility> facilities = service.GetAll();
            return facilities;
        }


        [HttpPost()]
        public bool InsertOne(Facility facility)
        {
            FacilityService service = new FacilityService();

            var res = service.InsertOne(facility);
            return res;
        }



    }
}
