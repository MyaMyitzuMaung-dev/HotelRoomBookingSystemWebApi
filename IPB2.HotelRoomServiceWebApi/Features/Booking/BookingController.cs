using IPB2.HotelRoomServiceWebApi.Features.Booking;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.HotelRoomServiceWebApi.Features.Booking
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookingService;

        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // Book Room
        [HttpPost]
        public async Task<IActionResult> BookRoom(BookRoomRequest request)
        {
            var response = await _bookingService.BookRoomAsync(request);
            if (!response.IsSuccess)
            {
                if (response.Message == "Room not found") return NotFound(response.Message);
                return BadRequest(response.Message);
            }
            return Ok(response);
        }

        // Booking List
        [HttpGet]
        public async Task<IActionResult> GetBookings(int pageNo = 1, int pageSize = 10)
        {
            var request = new GetBookingsRequest { PageNo = pageNo, PageSize = pageSize };
            var response = await _bookingService.GetBookingsAsync(request);
            return Ok(response.Data);
        }

        // Check-in
        [HttpPut("checkin/{id}")]
        public async Task<IActionResult> CheckIn(int id)
        {
            var request = new CheckInRequest { BookingId = id };
            var response = await _bookingService.CheckInAsync(request);
            if (!response.IsSuccess) return NotFound(response.Message);
            return Ok(response.Message);
        }

        // Check-out
        [HttpPut("checkout/{id}")]
        public async Task<IActionResult> CheckOut(int id)
        {
            var request = new CheckOutRequest { BookingId = id };
            var response = await _bookingService.CheckOutAsync(request);
            if (!response.IsSuccess) return NotFound(response.Message);
            return Ok(response.Message);
        }

        // Soft Delete
        [HttpPatch("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var request = new DeleteBookingRequest { BookingId = id };
            var response = await _bookingService.DeleteBookingAsync(request);
            if (!response.IsSuccess) return NotFound();
            return Ok(response.Message);
        }

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var request = new CancelBookingRequest { BookingId = id };
            var response = await _bookingService.CancelBookingAsync(request);
            if (!response.IsSuccess)
            {
                if (response.Message == "Booking not found") return NotFound(response.Message);
                return BadRequest(response.Message);
            }
            return Ok(response.Message);
        }
    }
}
