namespace DeacomTraining.POCOs
{
    public class Entry
    {
        public string ItemCode { get; set; } = null!;
        public int DestinationType  { get; set; } 
        public int DestinationId { get; set; }
        public decimal AdditionalQuantity { get; set; }
        public string Description { get; set; } = null!;

    }
}
