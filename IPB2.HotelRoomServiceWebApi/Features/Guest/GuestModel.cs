namespace IPB2.HotelRoomServiceWebApi.Features.Guest
{
    public class GetGuestsRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetGuestsResponse
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public List<IPB2.HotelRoomService.Database.AppDbContextModels.Guest> Data { get; set; } = new();
    }

    public class GetGuestByIdRequest
    {
        public int GuestId { get; set; }
    }

    public class GetGuestByIdResponse
    {
        public IPB2.HotelRoomService.Database.AppDbContextModels.Guest Guest { get; set; } = null!;
    }

    public class CreateGuestRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }

    public class CreateGuestResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public int GuestId { get; set; }
    }

    public class UpdateGuestRequest
    {
        public int GuestId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }

    public class UpdateGuestResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }

    public class DeleteGuestRequest
    {
        public int GuestId { get; set; }
    }

    public class DeleteGuestResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
