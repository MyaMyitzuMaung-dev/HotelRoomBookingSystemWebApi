using IPB2.HotelRoomService.Database.AppDbContextModels;
using IPB2.HotelRoomService.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPB2.HotelRoomServiceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GuestController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetGuests(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var guests = await _context.Guests
                .Where(g => !g.IsDeleted)
                .OrderBy(g => g.GuestId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = guests
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGuest(int id)
        {
            var guest = await _context.Guests
                .FirstOrDefaultAsync(g => g.GuestId == id && !g.IsDeleted);

            if (guest == null)
                return NotFound(new { IsSuccess = false, Message = "Guest not found" });

            return Ok(guest);
        }

        // Register Guest
        [HttpPost]
        public async Task<IActionResult> CreateGuest([FromBody] GuestCreateRequestDto dto)
        {
            var guest = new Guest
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            await _context.Guests.AddAsync(guest);
            await _context.SaveChangesAsync();

            return Ok(new GuestCreateResponseDto
            {
                IsSuccess = true,
                Message = "Guest registered successfully",
                GuestId = guest.GuestId
            });
        }

        // Update Guest
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGuest(int id, [FromBody] GuestUpdateRequestDto dto)
        {
            var guest = await _context.Guests.FindAsync(id);

            if (guest == null || guest.IsDeleted)
            {
                return NotFound(new GuestUpdateResponseDto
                {
                    IsSuccess = false,
                    Message = "Guest not found"
                });
            }

            guest.FirstName = dto.FirstName;
            guest.LastName = dto.LastName;
            guest.Phone = dto.Phone;
            guest.Email = dto.Email;
            guest.Address = dto.Address;

            await _context.SaveChangesAsync();

            return Ok(new GuestUpdateResponseDto
            {
                IsSuccess = true,
                Message = "Guest updated successfully"
            });
        }

        // Soft Delete Guest
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuest(int id)
        {
            var guest = await _context.Guests.FindAsync(id);

            if (guest == null || guest.IsDeleted)
            {
                return NotFound(new GuestUpdateResponseDto
                {
                    IsSuccess = false,
                    Message = "Guest not found"
                });
            }

            guest.IsDeleted = true;

            await _context.SaveChangesAsync();

            return Ok(new GuestUpdateResponseDto
            {
                IsSuccess = true,
                Message = "Guest deleted successfully"
            });
        }
    }
}
