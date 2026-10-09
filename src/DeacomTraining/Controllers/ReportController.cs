using System.Globalization;
using DeacomTraining.POCOs;
using DeacomTraining.Service;
using Microsoft.AspNetCore.Mvc;

namespace DeacomTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        [HttpGet("FullInventory")]
        public ActionResult<FullInventoryReport> GetFullInventoryReport()
        {
            ReportService service = new ReportService();
            FullInventoryReport report = service.GetFullInventoryReport();

            Response.Headers["X-Database-Query-Count"] =
                report.DatabaseQueryCount.ToString(CultureInfo.InvariantCulture);
            Response.Headers["Server-Timing"] =
                $"db;dur={report.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture)}";

            return Ok(report);
        }
    }
}
