using IPB2.HotelRoomService.Shared.Enums;

namespace IPB2.HotelRoomService.Shared.Models;

public class RoomCreateRequestDto
{
    public string RoomNumber { get; set; } = null!;
    public string? RoomType { get; set; }
    public decimal? PricePerNight { get; set; }
    public int? Capacity { get; set; }
    public RoomStatus Status { get; set; } = RoomStatus.Available;
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
