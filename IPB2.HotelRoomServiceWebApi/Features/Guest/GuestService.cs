using IPB2.HotelRoomService.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace IPB2.HotelRoomServiceWebApi.Features.Guest
{
    public class GuestService
    {
        private readonly AppDbContext _context;

        public GuestService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GetGuestsResponse> GetGuestsAsync(GetGuestsRequest request)
        {
            var guests = await _context.Guests
                .Where(g => !g.IsDeleted)
                .OrderBy(g => g.GuestId)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new GetGuestsResponse
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Data = guests
            };
        }

        public async Task<GetGuestByIdResponse?> GetGuestAsync(GetGuestByIdRequest request)
        {
            var guest = await _context.Guests
                .FirstOrDefaultAsync(g => g.GuestId == request.GuestId && !g.IsDeleted);

            if (guest == null) return null;

            return new GetGuestByIdResponse
            {
                Guest = guest
            };
        }

        public async Task<CreateGuestResponse> CreateGuestAsync(CreateGuestRequest request)
        {
            var guest = new IPB2.HotelRoomService.Database.AppDbContextModels.Guest
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Phone = request.Phone,
                Email = request.Email,
                Address = request.Address,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            await _context.Guests.AddAsync(guest);
            await _context.SaveChangesAsync();

            return new CreateGuestResponse
            {
                IsSuccess = true,
                Message = "Guest registered successfully",
                GuestId = guest.GuestId
            };
        }

        public async Task<UpdateGuestResponse> UpdateGuestAsync(UpdateGuestRequest request)
        {
            var guest = await _context.Guests.FindAsync(request.GuestId);

            if (guest == null || guest.IsDeleted)
            {
                return new UpdateGuestResponse
                {
                    IsSuccess = false,
                    Message = "Guest not found"
                };
            }

            guest.FirstName = request.FirstName;
            guest.LastName = request.LastName;
            guest.Phone = request.Phone;
            guest.Email = request.Email;
            guest.Address = request.Address;

            await _context.SaveChangesAsync();

            return new UpdateGuestResponse
            {
                IsSuccess = true,
                Message = "Guest updated successfully"
            };
        }

        public async Task<DeleteGuestResponse> DeleteGuestAsync(DeleteGuestRequest request)
        {
            var guest = await _context.Guests.FindAsync(request.GuestId);

            if (guest == null || guest.IsDeleted)
            {
                return new DeleteGuestResponse
                {
                    IsSuccess = false,
                    Message = "Guest not found"
                };
            }

            guest.IsDeleted = true;

            await _context.SaveChangesAsync();

            return new DeleteGuestResponse
            {
                IsSuccess = true,
                Message = "Guest deleted successfully"
            };
        }
    }
}
