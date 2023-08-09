using DeacomTraining.Data;
using System.Data.SqlClient;

namespace DeacomTraining.BusinessClasses
{
    public partial class Facility
    {
        public int fc_id { get; set; }
        public string fc_name { get; set; } = null!;
        public string fc_adrss { get; set; } = null!;
        public string fc_phone { get; set; } = null!;
        public string fc_desc { get; set; } = null!;

    }
}
