namespace IPB2.HotelRoomServiceWebApi.Models;

public class RoomCreateRequestDto
{
    public string RoomNumber { get; set; } = null!;
    public string? RoomType { get; set; }
    public decimal? PricePerNight { get; set; }
    public int? Capacity { get; set; }
    public string? Status { get; set; }
}
public class RoomCreateResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public int? RoomId { get; set; }
}
public class RoomUpdateRequestDto
{
    public string RoomNumber { get; set; } = null!;
    public string? RoomType { get; set; }
    public decimal? PricePerNight { get; set; }
    public int? Capacity { get; set; }
    public string? Status { get; set; }
}
public class RoomUpdateResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
}
public class RoomPatchRequestDto
{
    public string? Status { get; set; }
}
public class RoomPatchResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
}
