namespace DeacomTraining.BusinessClasses
{
    public class Item
    {
        public int it_id { get; set; }
        public string it_name { get; set; } = null!;
        public string it_code { get; set; } = null!;
        public string it_desc { get; set; } = null!;
        public int it_tpid { get; set; }
    }
}
