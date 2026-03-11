namespace IPB2.HotelRoomServiceWebApi.Models
{
    public class BookingReportDto
    {
        public int BookingId { get; set; }

        public string? RoomNumber { get; set; }

        public string? GuestName { get; set; }

        public DateOnly? CheckInDate { get; set; }

        public DateOnly? CheckOutDate { get; set; }

        public string? Status { get; set; }

        public decimal? TotalAmount { get; set; }
    }
}
