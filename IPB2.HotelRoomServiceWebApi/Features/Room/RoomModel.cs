namespace IPB2.HotelRoomServiceWebApi.Features.Room
{
    public class GetRoomsRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetRoomsResponse
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<IPB2.HotelRoomService.Database.AppDbContextModels.Room> Data { get; set; } = new();
    }

    public class GetRoomByIdRequest
    {
        public int RoomId { get; set; }
    }

    public class GetRoomByIdResponse
    {
        public IPB2.HotelRoomService.Database.AppDbContextModels.Room Room { get; set; } = null!;
    }

    public class CreateRoomRequest
    {
        public string? RoomNumber { get; set; }
        public string? RoomType { get; set; }
        public decimal? PricePerNight { get; set; }
        public int? Capacity { get; set; }
    }

    public class CreateRoomResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public int RoomId { get; set; }
    }

    public class UpdateRoomRequest
    {
        public int RoomId { get; set; }
        public string? RoomNumber { get; set; }
        public string? RoomType { get; set; }
        public decimal? PricePerNight { get; set; }
        public int? Capacity { get; set; }
        public string? Status { get; set; }
    }

    public class UpdateRoomResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }

    public class PatchRoomRequest
    {
        public int RoomId { get; set; }
        public string? Status { get; set; }
    }

    public class PatchRoomResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }

    public class DeleteRoomRequest
    {
        public int RoomId { get; set; }
    }

    public class DeleteRoomResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }

    public class SearchRoomsRequest
    {
        public string? RoomType { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class SearchRoomsResponse
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<IPB2.HotelRoomService.Database.AppDbContextModels.Room> Data { get; set; } = new();
    }
}
