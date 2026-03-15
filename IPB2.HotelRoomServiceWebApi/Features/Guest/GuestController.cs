using IPB2.HotelRoomServiceWebApi.Features.Guest;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.HotelRoomServiceWebApi.Features.Guest
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly GuestService _guestService;

        public GuestController(GuestService guestService)
        {
            _guestService = guestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGuests(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var request = new GetGuestsRequest { PageNumber = pageNumber, PageSize = pageSize };
            var response = await _guestService.GetGuestsAsync(request);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGuest(int id)
        {
            var request = new GetGuestByIdRequest { GuestId = id };
            var response = await _guestService.GetGuestAsync(request);

            if (response == null)
                return NotFound(new { IsSuccess = false, Message = "Guest not found" });

            return Ok(response.Guest);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGuest([FromBody] CreateGuestRequest request)
        {
            var response = await _guestService.CreateGuestAsync(request);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGuest(int id, [FromBody] UpdateGuestRequest request)
        {
            request.GuestId = id;
            var response = await _guestService.UpdateGuestAsync(request);

            if (!response.IsSuccess)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuest(int id)
        {
            var request = new DeleteGuestRequest { GuestId = id };
            var response = await _guestService.DeleteGuestAsync(request);

            if (!response.IsSuccess)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
