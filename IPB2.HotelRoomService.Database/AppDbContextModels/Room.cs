using System;
using System.Collections.Generic;

namespace IPB2.HotelRoomService.Database.AppDbContextModels;

public partial class Room
{
    public int RoomId { get; set; }

    public string RoomNumber { get; set; } = null!;

    public string? RoomType { get; set; }

    public decimal? PricePerNight { get; set; }

    public int? Capacity { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
