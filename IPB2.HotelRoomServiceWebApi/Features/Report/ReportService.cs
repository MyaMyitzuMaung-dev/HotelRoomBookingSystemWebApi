using IPB2.HotelRoomService.Database.AppDbContextModels;
using IPB2.HotelRoomService.Shared.Enums;
using IPB2.HotelRoomService.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace IPB2.HotelRoomServiceWebApi.Features.Report
{
    public class ReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GetBookingReportResponse> GetBookingReportAsync(GetBookingReportRequest request)
        {
            var totalCount = await _context.Bookings
                .Where(x => !x.IsDeleted)
                .CountAsync();

            var report = await _context.Bookings
                .Where(x => !x.IsDeleted)
                .Include(x => x.Room)
                .Include(x => x.Guest)
                .Skip((request.PageNo - 1) * request.PageSize)
                .Take(request.PageSize)
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

            return new GetBookingReportResponse
            {
                PageNo = request.PageNo,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                Data = report
            };
        }

        public async Task<GetDailyOccupancyResponse> GetDailyOccupancyAsync(GetDailyOccupancyRequest request)
        {
            var reportDate = DateOnly.FromDateTime(request.Date);

            var bookingsOnDate = await _context.Bookings
                .Include(b => b.Room)
                .Where(b => !b.IsDeleted
                            && b.Status == BookingStatus.CheckedIn.ToString()
                            && b.CheckInDate <= reportDate
                            && b.CheckOutDate >= reportDate)
                .ToListAsync();

            var result = bookingsOnDate.Select(b => new DailyOccupancyItem
            {
                BookingId = b.BookingId,
                RoomNumber = b.Room!.RoomNumber,
                RoomType = b.Room!.RoomType,
                GuestId = b.GuestId,
                CheckInDate = (DateOnly)b.CheckInDate!,
                CheckOutDate = (DateOnly)b.CheckOutDate!,
                Status = b.Status
            }).ToList();

            return new GetDailyOccupancyResponse
            {
                Date = reportDate,
                TotalOccupiedRooms = bookingsOnDate.Count,
                Bookings = result
            };
        }
    }
}
