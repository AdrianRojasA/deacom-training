using System.Text.Json.Serialization;

namespace DeacomTraining.POCOs
{
    public class FullInventoryReport
    {
        public List<InventoryReportItem> Items { get; set; } = new();
        public List<InventoryReportLocation> Warehouses { get; set; } = new();
        public List<InventoryReportLocation> Facilities { get; set; } = new();

        [JsonIgnore]
        public int DatabaseQueryCount { get; set; }

        [JsonIgnore]
        public long ElapsedMilliseconds { get; set; }
    }

    public class InventoryReportItem
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public InventoryReportType Type { get; set; } = new();
        public List<InventoryReportStock> WarehouseInventory { get; set; } = new();
        public List<InventoryReportStock> FacilityInventory { get; set; } = new();
        public decimal TotalQuantity { get; set; }
        public decimal TypeTotalQuantity { get; set; }
        public decimal TypeSharePercent { get; set; }
    }

    public class InventoryReportType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class InventoryReportLocation
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class InventoryReportStock
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
    }
}
