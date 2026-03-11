using IPB2.HotelRoomService.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPB2.HotelRoomService.Shared.Models
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
