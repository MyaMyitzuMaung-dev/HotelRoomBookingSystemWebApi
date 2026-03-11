using IPB2.HotelRoomService.Database.AppDbContextModels;
using IPB2.HotelRoomServiceWebApi.Enums;
using IPB2.HotelRoomServiceWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPB2.HotelRoomServiceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }
        // Booking Report
        [HttpGet("booking-report")]
        public async Task<IActionResult> GetBookingReport(int pageNo = 1, int pageSize = 10)
        {
            var totalCount = await _context.Bookings
                .Where(x => !x.IsDeleted)
                .CountAsync();

            var report = await _context.Bookings
                .Where(x => !x.IsDeleted)
                .Include(x => x.Room)
                .Include(x => x.Guest)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new BookingReportDto
                {
                    BookingId = x.BookingId,
                    RoomNumber = x.Room!.RoomNumber,
                    GuestName = x.Guest!.FirstName + " " + x.Guest.LastName,
                    CheckInDate = x.CheckInDate,
                    CheckOutDate = x.CheckOutDate,
                    Status = x.Status,
                    TotalAmount = x.TotalAmount
                })
                .ToListAsync();

            return Ok(new
            {
                pageNo,
                pageSize,
                totalCount,
                data = report
            });
        }
        // GET: api/report/daily-occupancy?date=2026-03-11
        [HttpGet("daily-occupancy")]
        public async Task<IActionResult> GetDailyOccupancy([FromQuery] DateTime date)
        {
            // Convert DateTime to DateOnly
            var reportDate = DateOnly.FromDateTime(date);

            // Get bookings for that date
            var bookingsOnDate = await _context.Bookings
                .Include(b => b.Room)
                .Where(b => !b.IsDeleted
                            && b.Status == BookingStatus.CheckedIn.ToString()
                            && b.CheckInDate <= reportDate
                            && b.CheckOutDate >= reportDate)
                .ToListAsync();

            // Build result
            var result = bookingsOnDate.Select(b => new
            {
                b.BookingId,
                RoomNumber = b.Room!.RoomNumber,
                RoomType = b.Room!.RoomType,
                GuestId = b.GuestId,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                Status = b.Status
            });

            return Ok(new
            {
                Date = reportDate,
                TotalOccupiedRooms = bookingsOnDate.Count,
                Bookings = result
            });
        }
    }
}
