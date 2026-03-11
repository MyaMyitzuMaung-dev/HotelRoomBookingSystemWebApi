namespace IPB2.HotelRoomServiceWebApi.Models
{
    public class BookingCreateRequestDto
    {
        public int RoomId { get; set; }
        public int GuestId { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
    }

    public class BookingCreateResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? BookingId { get; set; }
    }
    public class CheckInRequestDto
    {
        public int BookingId { get; set; }
    }

    public class CheckOutRequestDto
    {
        public int BookingId { get; set; }
    }
    public class BookingResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
