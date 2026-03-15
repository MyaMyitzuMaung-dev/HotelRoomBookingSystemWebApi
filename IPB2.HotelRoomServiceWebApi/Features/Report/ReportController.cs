using IPB2.HotelRoomServiceWebApi.Features.Report;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.HotelRoomServiceWebApi.Features.Report
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        // Booking Report
        [HttpGet("booking-report")]
        public async Task<IActionResult> GetBookingReport(int pageNo = 1, int pageSize = 10)
        {
            var request = new GetBookingReportRequest { PageNo = pageNo, PageSize = pageSize };
            var response = await _reportService.GetBookingReportAsync(request);
            return Ok(response);
        }

        // GET: api/report/daily-occupancy?date=2026-03-11
        [HttpGet("daily-occupancy")]
        public async Task<IActionResult> GetDailyOccupancy([FromQuery] DateTime date)
        {
            var request = new GetDailyOccupancyRequest { Date = date };
            var response = await _reportService.GetDailyOccupancyAsync(request);
            return Ok(response);
        }
    }
}
