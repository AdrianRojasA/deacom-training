using DeacomTraining.BusinessClasses;
using DeacomTraining.Data;
using System.Data;

namespace DeacomTraining.Service
{
    public class FacilityService : MainService
    {
        private Facility _facility = new Facility();

        public Facility GetOne(int id)
        {
            Facility facility = _facility.GetOne(connection, id);
            return facility;
        }

        public IEnumerable<Facility> GetAll()
        {
            var facs = new List<Facility>();
            return facs;
        }

        public bool InsertOne(Facility facility)
        {
            SqlExecute.ExecuteCommand($"INSERT INTO tnfclty(" +
                $"fc_name, fc_adrss, fc_phone, fc_desc) " +
                $"VALUES( {facility.fc_name}, {facility.fc_adrss}, " +
                $"{facility.fc_phone}, {facility.fc_desc})");
            return true;
        }

    }
}
