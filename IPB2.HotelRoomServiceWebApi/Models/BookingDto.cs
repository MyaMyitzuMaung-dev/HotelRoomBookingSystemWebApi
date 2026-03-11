using IPB2.HotelRoomServiceWebApi.Enums;

namespace IPB2.HotelRoomServiceWebApi.Models
{
    // Book Room
    public class BookingCreateRequestDto
    {
        public int RoomId { get; set; }
        public int GuestId { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
    }

    public class BookingCreateResponseDto
    {
        public int BookingId { get; set; }
        public string Message { get; set; } = "";
    }

    // Check-in
    public class CheckInRequestDto
    {
        public int BookingId { get; set; }
    }

    // Check-out
    public class CheckOutRequestDto
    {
        public int BookingId { get; set; }
    }

    // Booking List
    public class BookingResponseDto
    {
        public int BookingId { get; set; }
        public int? RoomId { get; set; }
        public int? GuestId { get; set; }
        public DateOnly? CheckInDate { get; set; }
        public DateOnly? CheckOutDate { get; set; }
        public BookingStatus Status { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
