using System.Data;
using System.Diagnostics;
using DeacomTraining.Data;
using DeacomTraining.POCOs;

namespace DeacomTraining.Service
{
    public class ReportService : MainService
    {
        private int _databaseQueryCount;

        public FullInventoryReport GetFullInventoryReport()
        {
            _databaseQueryCount = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();

            FullInventoryReport report = new FullInventoryReport
            {
                Warehouses = GetLocations(
                    "SELECT wh_id, wh_name, wh_adrss, wh_phone, wh_desc " +
                    "FROM tnwrhse ORDER BY wh_id",
                    "wh"),
                Facilities = GetLocations(
                    "SELECT fc_id, fc_name, fc_adrss, fc_phone, fc_desc " +
                    "FROM tnfclty ORDER BY fc_id",
                    "fc")
            };

            DataTable itemRows = RunQuery(
                "SELECT it.it_id, it.it_code, it.it_name, it.it_desc, it.it_tpid, " +
                "(SELECT COALESCE(SUM(iw.iw_quantity), 0) FROM tnitmwhs iw " +
                "JOIN tnitem t ON t.it_id = iw.iw_itid WHERE t.it_tpid = it.it_tpid) + " +
                "(SELECT COALESCE(SUM(fi.if_quantity), 0) FROM tnitmflty fi " +
                "JOIN tnitem t ON t.it_id = fi.if_itid WHERE t.it_tpid = it.it_tpid) AS type_total_quantity " +
                "FROM tnitem it ORDER BY it.it_id",
                "report-items");

            foreach (DataRow itemRow in itemRows.Rows)
            {
                int itemId = Convert.ToInt32(itemRow["it_id"]);
                string itemCode = GetString(itemRow, "it_code");

                InventoryReportItem item = new InventoryReportItem
                {
                    Id = itemId,
                    Code = itemCode,
                    Name = GetString(itemRow, "it_name"),
                    Description = GetString(itemRow, "it_desc"),
                    Type = GetItemType(Convert.ToInt32(itemRow["it_tpid"])),
                    WarehouseInventory = GetStock(
                        "SELECT iw.iw_whid AS location_id, w.wh_name AS location_name, " +
                        "iw.iw_quantity AS quantity " +
                        "FROM tnitmwhs iw " +
                        "JOIN tnwrhse w ON w.wh_id = iw.iw_whid " +
                        $"WHERE CAST(iw.iw_itid AS VARCHAR(10)) = '{itemId}' " +
                        "ORDER BY iw.iw_whid",
                        $"warehouse-stock-{itemId}"),
                    FacilityInventory = GetStock(
                        "SELECT fi.if_fcid AS location_id, f.fc_name AS location_name, " +
                        "SUM(fi.if_quantity) AS quantity " +
                        "FROM tnitmflty fi " +
                        "JOIN tnfclty f ON f.fc_id = fi.if_fcid " +
                        "GROUP BY fi.if_itid, fi.if_fcid, f.fc_name " +
                        $"HAVING fi.if_itid IN (SELECT it_id FROM tnitem WHERE it_code = '{itemCode}') " +
                        "ORDER BY fi.if_fcid",
                        $"facility-stock-{itemId}"),
                    TypeTotalQuantity = Convert.ToDecimal(itemRow["type_total_quantity"])
                };

                item.TotalQuantity =
                    item.WarehouseInventory.Sum(stock => stock.Quantity) +
                    item.FacilityInventory.Sum(stock => stock.Quantity);

                if (item.TypeTotalQuantity > 0)
                {
                    item.TypeSharePercent = Math.Round(item.TotalQuantity * 100 / item.TypeTotalQuantity, 4);
                }

                report.Items.Add(item);
            }

            stopwatch.Stop();
            report.DatabaseQueryCount = _databaseQueryCount;
            report.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            return report;
        }

        private InventoryReportType GetItemType(int typeId)
        {
            DataTable typeRows = RunQuery(
                "SELECT tp_id, tp_name, tp_desc " +
                $"FROM tntype WHERE tp_id = {typeId}",
                $"item-type-{typeId}");

            DataRow? typeRow = typeRows.Rows.Cast<DataRow>().FirstOrDefault();
            if (typeRow == null)
            {
                return new InventoryReportType { Id = typeId };
            }

            return new InventoryReportType
            {
                Id = Convert.ToInt32(typeRow["tp_id"]),
                Name = GetString(typeRow, "tp_name"),
                Description = GetString(typeRow, "tp_desc")
            };
        }

        private List<InventoryReportLocation> GetLocations(string sql, string prefix)
        {
            DataTable rows = RunQuery(sql, $"{prefix}-locations");
            return rows.Rows.Cast<DataRow>()
                .Select(row => new InventoryReportLocation
                {
                    Id = Convert.ToInt32(row[$"{prefix}_id"]),
                    Name = GetString(row, $"{prefix}_name"),
                    Address = GetString(row, $"{prefix}_adrss"),
                    Phone = GetString(row, $"{prefix}_phone"),
                    Description = GetString(row, $"{prefix}_desc")
                })
                .ToList();
        }

        private List<InventoryReportStock> GetStock(string sql, string tableName)
        {
            DataTable rows = RunQuery(sql, tableName);
            return rows.Rows.Cast<DataRow>()
                .Select(row => new InventoryReportStock
                {
                    LocationId = Convert.ToInt32(row["location_id"]),
                    LocationName = GetString(row, "location_name"),
                    Quantity = Convert.ToDecimal(row["quantity"])
                })
                .ToList();
        }

        private DataTable RunQuery(string sql, string tableName)
        {
            _databaseQueryCount++;
            return Cursor.ToCursor(sql, tableName, connection);
        }

        private static string GetString(DataRow row, string columnName)
        {
            return row.IsNull(columnName) ? string.Empty : Convert.ToString(row[columnName]) ?? string.Empty;
        }
    }
}
