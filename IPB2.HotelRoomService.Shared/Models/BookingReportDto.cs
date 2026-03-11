using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPB2.HotelRoomService.Shared.Models
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
    public class DailyOccupancyReportDto
    {
        public DateTime Date { get; set; }           // Report date
        public int TotalRooms { get; set; }          // Total rooms
        public int OccupiedRooms { get; set; }       // Number of occupied rooms
        public int AvailableRooms { get; set; }      // Number of available rooms
        public int MaintenanceRooms { get; set; }    // Number of rooms under maintenance
    }
}
