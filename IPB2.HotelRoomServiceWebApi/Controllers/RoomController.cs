using IPB2.HotelRoomService.Database.AppDbContextModels;
using IPB2.HotelRoomService.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPB2.HotelRoomServiceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoomController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/room
        [HttpGet]
        public async Task<IActionResult> GetRooms(
        [FromQuery] int pageNumber = 1,  // default page 1
        [FromQuery] int pageSize = 10)   // default 10 items per page
        {
            var query = _context.Rooms.Where(r => !r.IsDeleted);

            var rooms = await query
                .OrderBy(r => r.RoomId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = rooms
            });
        }

        // GET: api/room/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            var room = await _context.Rooms
                                     .FirstOrDefaultAsync(r => r.RoomId == id && !r.IsDeleted);
            if (room == null)
                return NotFound(new { IsSuccess = false, Message = "Room not found" });

            return Ok(room);
        }

        // POST: api/room
        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] RoomCreateRequestDto dto)
        {
            var room = new Room
            {
                RoomNumber = dto.RoomNumber,
                RoomType = dto.RoomType,
                PricePerNight = dto.PricePerNight,
                Capacity = dto.Capacity,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();

            return Ok(new RoomCreateResponseDto
            {
                IsSuccess = true,
                Message = "Room created successfully",
                RoomId = room.RoomId
            });
        }

        // PUT: api/room/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomUpdateRequestDto dto)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null || room.IsDeleted)
            {
                return NotFound(new RoomUpdateResponseDto
                {
                    IsSuccess = false,
                    Message = "Room not found"
                });
            }

            room.RoomNumber = dto.RoomNumber;
            room.RoomType = dto.RoomType;
            room.PricePerNight = dto.PricePerNight;
            room.Capacity = dto.Capacity;
            room.Status = dto.Status ?? room.Status;

            await _context.SaveChangesAsync();

            return Ok(new RoomUpdateResponseDto
            {
                IsSuccess = true,
                Message = "Room updated successfully"
            });
        }

        // PATCH: api/room/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchRoom(int id, [FromBody] RoomPatchRequestDto dto)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null || room.IsDeleted)
            {
                return NotFound(new RoomPatchResponseDto
                {
                    IsSuccess = false,
                    Message = "Room not found"
                });
            }

            if (!string.IsNullOrEmpty(dto.Status))
                room.Status = dto.Status;

            await _context.SaveChangesAsync();

            return Ok(new RoomPatchResponseDto
            {
                IsSuccess = true,
                Message = "Room status updated successfully"
            });
        }

        // DELETE: api/room/5  (Soft Delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null || room.IsDeleted)
            {
                return NotFound(new RoomUpdateResponseDto
                {
                    IsSuccess = false,
                    Message = "Room not found"
                });
            }

            room.IsDeleted = true;
            await _context.SaveChangesAsync();

            return Ok(new RoomUpdateResponseDto
            {
                IsSuccess = true,
                Message = "Room soft-deleted successfully"
            });
        }

        // GET: api/room/search
        [HttpGet("search")]
        public async Task<IActionResult> SearchRooms(
            [FromQuery] string? roomType,          // filter by room type
            [FromQuery] int pageNumber = 1,        
            [FromQuery] int pageSize = 10)        
        {
            var query = _context.Rooms.Where(r => !r.IsDeleted);

            if (!string.IsNullOrEmpty(roomType))
            {
                query = query.Where(r => r.RoomType != null && r.RoomType.Contains(roomType));
            }

            var totalCount = await query.CountAsync();

            var rooms = await query
                .OrderBy(r => r.RoomNumber)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = rooms
            });
        }
    }
}