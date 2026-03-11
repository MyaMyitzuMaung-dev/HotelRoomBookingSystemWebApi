using IPB2.HotelRoomService.Database.AppDbContextModels;
using IPB2.HotelRoomService.Shared.Enums;
using IPB2.HotelRoomService.Shared.Models;
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
        // Book Room
        [HttpPost]
        public async Task<IActionResult> BookRoom(BookingCreateRequestDto dto)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomId == dto.RoomId && !r.IsDeleted);

            if (room == null)
                return NotFound("Room not found");

            // calculate nights
            var nights = dto.CheckOutDate.DayNumber - dto.CheckInDate.DayNumber;

            if (nights <= 0)
                return BadRequest("Invalid check-in and check-out date");

            // calculate total amount
            decimal totalAmount = (room.PricePerNight ?? 0) * nights;

            var booking = new Booking
            {
                RoomId = dto.RoomId,
                GuestId = dto.GuestId,
                BookingDate = DateTime.Now,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                TotalAmount = totalAmount,
                Status = BookingStatus.Booked.ToString(),
                IsDeleted = false
            };

            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            return Ok(new BookingCreateResponseDto
            {
                BookingId = booking.BookingId,
                Message = $"Room booked successfully. Total Amount = {totalAmount}"
            });
        }

        // Booking List
        [HttpGet]
        public async Task<IActionResult> GetBookings(int pageNo = 1, int pageSize = 10)
        {
            var bookings = await _context.Bookings
                .Where(x => !x.IsDeleted)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new BookingResponseDto
                {
                    BookingId = x.BookingId,
                    RoomId = x.RoomId,
                    GuestId = x.GuestId,
                    CheckInDate = x.CheckInDate,
                    CheckOutDate = x.CheckOutDate,
                    Status = Enum.Parse<BookingStatus>(x.Status!),
                    TotalAmount = x.TotalAmount
                })
                .ToListAsync();

            return Ok(bookings);
        }

        // Check-in
        [HttpPut("checkin/{id}")]
        public async Task<IActionResult> CheckIn(int id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(x => x.BookingId == id && !x.IsDeleted);

            if (booking == null)
                return NotFound("Booking not found");

            booking.Status = BookingStatus.CheckedIn.ToString();

            await _context.SaveChangesAsync();

            return Ok("Check-in successful");
        }

        // Check-out
        [HttpPut("checkout/{id}")]
        public async Task<IActionResult> CheckOut(int id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(x => x.BookingId == id && !x.IsDeleted);

            if (booking == null)
                return NotFound("Booking not found");

            booking.Status = BookingStatus.CheckedOut.ToString();

            await _context.SaveChangesAsync();

            return Ok("Check-out successful");
        }

        // Soft Delete
        [HttpPatch("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(x => x.BookingId == id);

            if (booking == null)
                return NotFound();

            booking.IsDeleted = true;

            await _context.SaveChangesAsync();

            return Ok("Booking deleted");
        }

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(x => x.BookingId == id && !x.IsDeleted);

            if (booking == null)
                return NotFound("Booking not found");

            if (booking.Status == BookingStatus.CheckedIn.ToString())
                return BadRequest("Cannot cancel after check-in");

            booking.Status = BookingStatus.Cancelled.ToString();

            await _context.SaveChangesAsync();

            return Ok("Booking cancelled successfully");
        }
    }
}
