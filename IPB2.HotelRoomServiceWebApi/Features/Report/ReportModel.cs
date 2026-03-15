using IPB2.HotelRoomService.Shared.Models;

namespace IPB2.HotelRoomServiceWebApi.Features.Report
{
    public class GetBookingReportRequest
    {
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetBookingReportResponse
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<BookingReportDto> Data { get; set; } = new();
    }

    public class GetDailyOccupancyRequest
    {
        public DateTime Date { get; set; }
    }

    public class GetDailyOccupancyResponse
    {
        public DateOnly Date { get; set; }
        public int TotalOccupiedRooms { get; set; }
        public List<DailyOccupancyItem> Bookings { get; set; } = new();
    }

    public class DailyOccupancyItem
    {
        public int BookingId { get; set; }
        public string? RoomNumber { get; set; }
        public string? RoomType { get; set; }
        public int? GuestId { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public string? Status { get; set; }
    }
}
