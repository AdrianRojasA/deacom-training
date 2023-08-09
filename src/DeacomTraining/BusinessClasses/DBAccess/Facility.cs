using DeacomTraining.BusinessClasses.Interfaces;
using DeacomTraining.Data.Extensions;
using System.Data.Common;

namespace DeacomTraining.BusinessClasses
{
    public partial class Facility : IDBAccess<Facility>
    {
        public Facility GetOne(DbConnection db, int id)
        {
            string query = "SELECT * FROM tnfclty WHERE fc_id = " + id;
            Facility facility = new Facility();

            var cmd = db.CreateCommand();
            cmd.CommandText = query;

            try
            {
                var reader = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow);
                while (reader.Read())
                {
                    Facility dbResponse = new Facility();
                    dbResponse.fc_id = (int)reader["fc_id"];
                    dbResponse.fc_name = (string)reader["fc_name"];
                    dbResponse.fc_adrss = (string)reader["fc_adrss"];
                    dbResponse.fc_phone = (string)reader["fc_phone"];
                    dbResponse.fc_desc = (string)reader["fc_desc"];
                    facility = dbResponse;
                }
                reader.Close();
                return facility;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
