using IPB2.HotelRoomService.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace IPB2.HotelRoomServiceWebApi.Features.Room
{
    public class RoomService
    {
        private readonly AppDbContext _context;

        public RoomService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GetRoomsResponse> GetRoomsAsync(GetRoomsRequest request)
        {
            var query = _context.Rooms.Where(r => !r.IsDeleted);

            var rooms = await query
                .OrderBy(r => r.RoomId)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new GetRoomsResponse
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Data = rooms
            };
        }

        public async Task<GetRoomByIdResponse?> GetRoomAsync(GetRoomByIdRequest request)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomId == request.RoomId && !r.IsDeleted);

            if (room == null) return null;

            return new GetRoomByIdResponse
            {
                Room = room
            };
        }

        public async Task<CreateRoomResponse> CreateRoomAsync(CreateRoomRequest request)
        {
            var room = new IPB2.HotelRoomService.Database.AppDbContextModels.Room
            {
                RoomNumber = request.RoomNumber,
                RoomType = request.RoomType,
                PricePerNight = request.PricePerNight,
                Capacity = request.Capacity,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();

            return new CreateRoomResponse
            {
                IsSuccess = true,
                Message = "Room created successfully",
                RoomId = room.RoomId
            };
        }

        public async Task<UpdateRoomResponse> UpdateRoomAsync(UpdateRoomRequest request)
        {
            var room = await _context.Rooms.FindAsync(request.RoomId);
            if (room == null || room.IsDeleted)
            {
                return new UpdateRoomResponse
                {
                    IsSuccess = false,
                    Message = "Room not found"
                };
            }

            room.RoomNumber = request.RoomNumber;
            room.RoomType = request.RoomType;
            room.PricePerNight = request.PricePerNight;
            room.Capacity = request.Capacity;
            room.Status = request.Status ?? room.Status;

            await _context.SaveChangesAsync();

            return new UpdateRoomResponse
            {
                IsSuccess = true,
                Message = "Room updated successfully"
            };
        }

        public async Task<PatchRoomResponse> PatchRoomAsync(PatchRoomRequest request)
        {
            var room = await _context.Rooms.FindAsync(request.RoomId);
            if (room == null || room.IsDeleted)
            {
                return new PatchRoomResponse
                {
                    IsSuccess = false,
                    Message = "Room not found"
                };
            }

            if (!string.IsNullOrEmpty(request.Status))
                room.Status = request.Status;

            await _context.SaveChangesAsync();

            return new PatchRoomResponse
            {
                IsSuccess = true,
                Message = "Room status updated successfully"
            };
        }

        public async Task<DeleteRoomResponse> DeleteRoomAsync(DeleteRoomRequest request)
        {
            var room = await _context.Rooms.FindAsync(request.RoomId);
            if (room == null || room.IsDeleted)
            {
                return new DeleteRoomResponse
                {
                    IsSuccess = false,
                    Message = "Room not found"
                };
            }

            room.IsDeleted = true;
            await _context.SaveChangesAsync();

            return new DeleteRoomResponse
            {
                IsSuccess = true,
                Message = "Room soft-deleted successfully"
            };
        }

        public async Task<SearchRoomsResponse> SearchRoomsAsync(SearchRoomsRequest request)
        {
            var query = _context.Rooms.Where(r => !r.IsDeleted);

            if (!string.IsNullOrEmpty(request.RoomType))
            {
                query = query.Where(r => r.RoomType != null && r.RoomType.Contains(request.RoomType));
            }

            var totalCount = await query.CountAsync();

            var rooms = await query
                .OrderBy(r => r.RoomNumber)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new SearchRoomsResponse
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                Data = rooms
            };
        }
    }
}
