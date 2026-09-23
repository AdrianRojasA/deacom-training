using DeacomTraining.BusinessClasses;
using DeacomTraining.Service;
using Microsoft.AspNetCore.Mvc;

namespace DeacomTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilityController : ControllerBase
    {
        private FacilityService _service;

        public FacilityController()
        {
            _service = new FacilityService();
        }

        [HttpGet]
        public List<Facility> GetAll()
        {
            return _service.GetAllFacilities();
        }

        [HttpPost]
        public Facility Create(Facility facility)
        {
            _service.InsertFacility(facility);
            return facility;
        }

        [HttpPut("{id}")]
        public void Update(int id, Facility facility)
        {
            facility.fac_id = id;
            _service.UpdateFacility(facility);
        }
    }
}
