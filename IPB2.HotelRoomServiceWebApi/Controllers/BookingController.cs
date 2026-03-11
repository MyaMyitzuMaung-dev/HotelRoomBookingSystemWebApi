using IPB2.HotelRoomService.Database.AppDbContextModels;
using IPB2.HotelRoomServiceWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPB2.HotelRoomServiceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }
        // 1️⃣ Book Room
        [HttpPost("book")]
        public async Task<IActionResult> BookRoom([FromBody] BookingCreateRequestDto dto)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomId == dto.RoomId && !r.IsDeleted);

            if (room == null)
                return NotFound(new BookingCreateResponseDto
                {
                    IsSuccess = false,
                    Message = "Room not found"
                });

            var guest = await _context.Guests
                .FirstOrDefaultAsync(g => g.GuestId == dto.GuestId && !g.IsDeleted);

            if (guest == null)
                return NotFound(new BookingCreateResponseDto
                {
                    IsSuccess = false,
                    Message = "Guest not found"
                });

            var booking = new Booking
            {
                RoomId = dto.RoomId,
                GuestId = dto.GuestId,
                BookingDate = DateTime.Now,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                Status = "Booked",
                IsDeleted = false
            };

            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            return Ok(new BookingCreateResponseDto
            {
                IsSuccess = true,
                Message = "Room booked successfully",
                BookingId = booking.BookingId
            });
        }

        // 2️⃣ Check-in
        [HttpPost("checkin/{bookingId}")]
        public async Task<IActionResult> CheckIn(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId && !b.IsDeleted);

            if (booking == null)
                return NotFound(new BookingResponseDto
                {
                    IsSuccess = false,
                    Message = "Booking not found"
                });

            booking.Status = "CheckedIn";

            if (booking.Room != null)
                booking.Room.Status = "Occupied";

            await _context.SaveChangesAsync();

            return Ok(new BookingResponseDto
            {
                IsSuccess = true,
                Message = "Check-in successful"
            });
        }

        // 3️⃣ Check-out
        [HttpPost("checkout/{bookingId}")]
        public async Task<IActionResult> CheckOut(int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId && !b.IsDeleted);

            if (booking == null)
                return NotFound(new BookingResponseDto
                {
                    IsSuccess = false,
                    Message = "Booking not found"
                });

            booking.Status = "CheckedOut";

            if (booking.Room != null)
                booking.Room.Status = "Available";

            await _context.SaveChangesAsync();

            return Ok(new BookingResponseDto
            {
                IsSuccess = true,
                Message = "Check-out successful"
            });
        }
    }
}
