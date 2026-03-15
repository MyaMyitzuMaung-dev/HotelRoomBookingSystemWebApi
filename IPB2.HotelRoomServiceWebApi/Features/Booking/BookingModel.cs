using IPB2.HotelRoomService.Shared.Enums;

namespace IPB2.HotelRoomServiceWebApi.Features.Booking
{
    public class BookRoomRequest
    {
        public int RoomId { get; set; }
        public int GuestId { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
    }

    public class BookRoomResponse
    {
        public bool IsSuccess { get; set; }
        public int BookingId { get; set; }
        public string? Message { get; set; }
    }

    public class GetBookingsRequest
    {
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetBookingsResponse
    {
        public List<BookingItem> Data { get; set; } = new();
    }

    public class BookingItem
    {
        public int BookingId { get; set; }
        public int? RoomId { get; set; }
        public int? GuestId { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public BookingStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class CheckInRequest
    {
        public int BookingId { get; set; }
    }

    public class CheckInResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }

    public class CheckOutRequest
    {
        public int BookingId { get; set; }
    }

    public class CheckOutResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }

    public class DeleteBookingRequest
    {
        public int BookingId { get; set; }
    }

    public class DeleteBookingResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }

    public class CancelBookingRequest
    {
        public int BookingId { get; set; }
    }

    public class CancelBookingResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
