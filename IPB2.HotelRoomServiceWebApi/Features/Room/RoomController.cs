using IPB2.HotelRoomServiceWebApi.Features.Room;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.HotelRoomServiceWebApi.Features.Room
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        // GET: api/room
        [HttpGet]
        public async Task<IActionResult> GetRooms(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
        {
            var request = new GetRoomsRequest { PageNumber = pageNumber, PageSize = pageSize };
            var response = await _roomService.GetRoomsAsync(request);
            return Ok(response);
        }

        // GET: api/room/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            var request = new GetRoomByIdRequest { RoomId = id };
            var response = await _roomService.GetRoomAsync(request);
            if (response == null)
                return NotFound(new { IsSuccess = false, Message = "Room not found" });

            return Ok(response.Room);
        }

        // POST: api/room
        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest request)
        {
            var response = await _roomService.CreateRoomAsync(request);
            return Ok(response);
        }

        // PUT: api/room/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] UpdateRoomRequest request)
        {
            request.RoomId = id;
            var response = await _roomService.UpdateRoomAsync(request);
            if (!response.IsSuccess) return NotFound(response);
            return Ok(response);
        }

        // PATCH: api/room/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchRoom(int id, [FromBody] PatchRoomRequest request)
        {
            request.RoomId = id;
            var response = await _roomService.PatchRoomAsync(request);
            if (!response.IsSuccess) return NotFound(response);
            return Ok(response);
        }

        // DELETE: api/room/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var request = new DeleteRoomRequest { RoomId = id };
            var response = await _roomService.DeleteRoomAsync(request);
            if (!response.IsSuccess) return NotFound(response);
            return Ok(response);
        }

        // GET: api/room/search
        [HttpGet("search")]
        public async Task<IActionResult> SearchRooms(
            [FromQuery] string? roomType,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var request = new SearchRoomsRequest { RoomType = roomType, PageNumber = pageNumber, PageSize = pageSize };
            var response = await _roomService.SearchRoomsAsync(request);
            return Ok(response);
        }
    }
}
