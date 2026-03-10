using System;
using System.Collections.Generic;

namespace IPB2.HotelRoomService.Database.AppDbContextModels;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? RoomId { get; set; }

    public int? GuestId { get; set; }

    public DateTime? BookingDate { get; set; }

    public DateOnly? CheckInDate { get; set; }

    public DateOnly? CheckOutDate { get; set; }

    public string? Status { get; set; }

    public decimal? TotalAmount { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Guest? Guest { get; set; }

    public virtual Room? Room { get; set; }
}
