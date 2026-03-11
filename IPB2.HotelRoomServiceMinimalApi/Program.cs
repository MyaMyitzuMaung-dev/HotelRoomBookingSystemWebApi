using IPB2.HotelRoomService.Database.AppDbContextModels;
using IPB2.HotelRoomService.Shared.Enums;
using IPB2.HotelRoomService.Shared.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region Room Endpoints

// GET: api/rooms with pagination
app.MapGet("/api/rooms", async (AppDbContext db,
    int pageNumber = 1,
    int pageSize = 10) =>
{
    var query = db.Rooms.Where(r => !r.IsDeleted);

    var totalCount = await query.CountAsync();

    var rooms = await query
        .OrderBy(r => r.RoomId)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return Results.Ok(new
    {
        PageNumber = pageNumber,
        PageSize = pageSize,
        TotalCount = totalCount,
        Data = rooms
    });
});

// GET: api/rooms/{id}
app.MapGet("/api/rooms/{id:int}", async (AppDbContext db, int id) =>
{
    var room = await db.Rooms.FirstOrDefaultAsync(r => r.RoomId == id && !r.IsDeleted);
    return room is not null ? Results.Ok(room) : Results.NotFound(new { IsSuccess = false, Message = "Room not found" });
});

// POST: api/rooms
app.MapPost("/api/rooms", async (AppDbContext db, RoomCreateRequestDto dto) =>
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

    await db.Rooms.AddAsync(room);
    await db.SaveChangesAsync();

    return Results.Ok(new RoomCreateResponseDto
    {
        IsSuccess = true,
        Message = "Room created successfully",
        RoomId = room.RoomId
    });
});

// PUT: api/rooms/{id}
app.MapPut("/api/rooms/{id:int}", async (AppDbContext db, int id, RoomUpdateRequestDto dto) =>
{
    var room = await db.Rooms.FindAsync(id);
    if (room is null || room.IsDeleted)
        return Results.NotFound(new RoomUpdateResponseDto { IsSuccess = false, Message = "Room not found" });

    room.RoomNumber = dto.RoomNumber;
    room.RoomType = dto.RoomType;
    room.PricePerNight = dto.PricePerNight;
    room.Capacity = dto.Capacity;
    room.Status = dto.Status ?? room.Status;

    await db.SaveChangesAsync();
    return Results.Ok(new RoomUpdateResponseDto { IsSuccess = true, Message = "Room updated successfully" });
});

// PATCH: api/rooms/{id}
app.MapPatch("/api/rooms/{id:int}", async (AppDbContext db, int id, RoomPatchRequestDto dto) =>
{
    var room = await db.Rooms.FindAsync(id);
    if (room is null || room.IsDeleted)
        return Results.NotFound(new RoomPatchResponseDto { IsSuccess = false, Message = "Room not found" });

    if (!string.IsNullOrEmpty(dto.Status))
        room.Status = dto.Status;

    await db.SaveChangesAsync();
    return Results.Ok(new RoomPatchResponseDto { IsSuccess = true, Message = "Room status updated successfully" });
});

// DELETE: api/rooms/{id} (soft delete)
app.MapDelete("/api/rooms/{id:int}", async (AppDbContext db, int id) =>
{
    var room = await db.Rooms.FindAsync(id);
    if (room is null || room.IsDeleted)
        return Results.NotFound(new RoomUpdateResponseDto { IsSuccess = false, Message = "Room not found" });

    room.IsDeleted = true;
    await db.SaveChangesAsync();
    return Results.Ok(new RoomUpdateResponseDto { IsSuccess = true, Message = "Room soft-deleted successfully" });
});

// GET: api/rooms/search
app.MapGet("/api/rooms/search", async (AppDbContext db,
    string? roomType,
    int pageNumber = 1,
    int pageSize = 10) =>
{
    var query = db.Rooms.Where(r => !r.IsDeleted);

    if (!string.IsNullOrEmpty(roomType))
        query = query.Where(r => r.RoomType != null && r.RoomType.Contains(roomType));

    var totalCount = await query.CountAsync();

    var rooms = await query
        .OrderBy(r => r.RoomNumber)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return Results.Ok(new
    {
        PageNumber = pageNumber,
        PageSize = pageSize,
        TotalCount = totalCount,
        Data = rooms
    });
});

#endregion

#region Guest Endpoints

// GET: Guests with pagination
app.MapGet("/api/guests", async (AppDbContext context, int pageNumber = 1, int pageSize = 10) =>
{
    var guests = await context.Guests
        .Where(g => !g.IsDeleted)
        .OrderBy(g => g.GuestId)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return Results.Ok(new
    {
        PageNumber = pageNumber,
        PageSize = pageSize,
        Data = guests
    });
});

// GET: Single Guest by ID
app.MapGet("/api/guests/{id}", async (AppDbContext context, int id) =>
{
    var guest = await context.Guests
        .FirstOrDefaultAsync(g => g.GuestId == id && !g.IsDeleted);

    if (guest == null)
        return Results.NotFound(new { IsSuccess = false, Message = "Guest not found" });

    return Results.Ok(guest);
});

// POST: Register Guest
app.MapPost("/api/guests", async (AppDbContext context, GuestCreateRequestDto dto) =>
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

    await context.Guests.AddAsync(guest);
    await context.SaveChangesAsync();

    return Results.Ok(new GuestCreateResponseDto
    {
        IsSuccess = true,
        Message = "Guest registered successfully",
        GuestId = guest.GuestId
    });
});

// PUT: Update Guest
app.MapPut("/api/guests/{id}", async (AppDbContext context, int id, GuestUpdateRequestDto dto) =>
{
    var guest = await context.Guests.FindAsync(id);

    if (guest == null || guest.IsDeleted)
        return Results.NotFound(new GuestUpdateResponseDto
        {
            IsSuccess = false,
            Message = "Guest not found"
        });

    guest.FirstName = dto.FirstName;
    guest.LastName = dto.LastName;
    guest.Phone = dto.Phone;
    guest.Email = dto.Email;
    guest.Address = dto.Address;

    await context.SaveChangesAsync();

    return Results.Ok(new GuestUpdateResponseDto
    {
        IsSuccess = true,
        Message = "Guest updated successfully"
    });
});

// DELETE: Soft Delete Guest
app.MapDelete("/api/guests/{id}", async (AppDbContext context, int id) =>
{
    var guest = await context.Guests.FindAsync(id);

    if (guest == null || guest.IsDeleted)
        return Results.NotFound(new GuestUpdateResponseDto
        {
            IsSuccess = false,
            Message = "Guest not found"
        });

    guest.IsDeleted = true;
    await context.SaveChangesAsync();

    return Results.Ok(new GuestUpdateResponseDto
    {
        IsSuccess = true,
        Message = "Guest deleted successfully"
    });
});

#endregion

#region Booking Endpoints

// POST: Book Room
app.MapPost("/api/bookings", async (AppDbContext context, BookingCreateRequestDto dto) =>
{
    var room = await context.Rooms
        .FirstOrDefaultAsync(r => r.RoomId == dto.RoomId && !r.IsDeleted);

    if (room == null)
        return Results.NotFound("Room not found");

    var nights = dto.CheckOutDate.DayNumber - dto.CheckInDate.DayNumber;

    if (nights <= 0)
        return Results.BadRequest("Invalid check-in and check-out date");

    decimal totalAmount = (room.PricePerNight ?? 0) * nights;

    var booking = new Booking
    {
        RoomId = dto.RoomId,
        GuestId = dto.GuestId,
        BookingDate = DateTime.Now,
        CheckInDate = dto.CheckInDate,
        CheckOutDate = dto.CheckOutDate,
        TotalAmount = totalAmount,
        Status = BookingStatus.Booked.ToString(),
        IsDeleted = false
    };

    await context.Bookings.AddAsync(booking);
    await context.SaveChangesAsync();

    return Results.Ok(new BookingCreateResponseDto
    {
        BookingId = booking.BookingId,
        Message = $"Room booked successfully. Total Amount = {totalAmount}"
    });
});

// GET: Booking List with pagination
app.MapGet("/api/bookings", async (AppDbContext context, int pageNo = 1, int pageSize = 10) =>
{
    var bookings = await context.Bookings
        .Where(x => !x.IsDeleted)
        .Skip((pageNo - 1) * pageSize)
        .Take(pageSize)
        .Select(x => new BookingResponseDto
        {
            BookingId = x.BookingId,
            RoomId = x.RoomId,
            GuestId = x.GuestId,
            CheckInDate = x.CheckInDate,
            CheckOutDate = x.CheckOutDate,
            Status = Enum.Parse<BookingStatus>(x.Status!),
            TotalAmount = x.TotalAmount
        })
        .ToListAsync();

    return Results.Ok(bookings);
});

// PUT: Check-in
app.MapPut("/api/bookings/checkin/{id}", async (AppDbContext context, int id) =>
{
    var booking = await context.Bookings
        .FirstOrDefaultAsync(x => x.BookingId == id && !x.IsDeleted);

    if (booking == null)
        return Results.NotFound("Booking not found");

    booking.Status = BookingStatus.CheckedIn.ToString();
    await context.SaveChangesAsync();

    return Results.Ok("Check-in successful");
});

// PUT: Check-out
app.MapPut("/api/bookings/checkout/{id}", async (AppDbContext context, int id) =>
{
    var booking = await context.Bookings
        .FirstOrDefaultAsync(x => x.BookingId == id && !x.IsDeleted);

    if (booking == null)
        return Results.NotFound("Booking not found");

    booking.Status = BookingStatus.CheckedOut.ToString();
    await context.SaveChangesAsync();

    return Results.Ok("Check-out successful");
});

// PATCH: Soft Delete Booking
app.MapPatch("/api/bookings/{id}", async (AppDbContext context, int id) =>
{
    var booking = await context.Bookings
        .FirstOrDefaultAsync(x => x.BookingId == id);

    if (booking == null)
        return Results.NotFound();

    booking.IsDeleted = true;
    await context.SaveChangesAsync();

    return Results.Ok("Booking deleted");
});

// PUT: Cancel Booking
app.MapPut("/api/bookings/cancel/{id}", async (AppDbContext context, int id) =>
{
    var booking = await context.Bookings
        .FirstOrDefaultAsync(x => x.BookingId == id && !x.IsDeleted);

    if (booking == null)
        return Results.NotFound("Booking not found");

    if (booking.Status == BookingStatus.CheckedIn.ToString())
        return Results.BadRequest("Cannot cancel after check-in");

    booking.Status = BookingStatus.Cancelled.ToString();
    await context.SaveChangesAsync();

    return Results.Ok("Booking cancelled successfully");
});

#endregion

#region Report Endpoints

// GET: Booking Report with pagination
app.MapGet("/api/report/booking-report", async (AppDbContext context, int pageNo = 1, int pageSize = 10) =>
{
    var totalCount = await context.Bookings
        .Where(x => !x.IsDeleted)
        .CountAsync();

    var report = await context.Bookings
        .Where(x => !x.IsDeleted)
        .Include(x => x.Room)
        .Include(x => x.Guest)
        .Skip((pageNo - 1) * pageSize)
        .Take(pageSize)
        .Select(x => new BookingReportDto
        {
            BookingId = x.BookingId,
            RoomNumber = x.Room!.RoomNumber,
            GuestName = x.Guest!.FirstName + " " + x.Guest.LastName,
            CheckInDate = x.CheckInDate,
            CheckOutDate = x.CheckOutDate,
            Status = x.Status,
            TotalAmount = x.TotalAmount
        })
        .ToListAsync();

    return Results.Ok(new
    {
        pageNo,
        pageSize,
        totalCount,
        data = report
    });
});

// GET: Daily Occupancy Report
// Example: /api/report/daily-occupancy?date=2026-03-11
app.MapGet("/api/report/daily-occupancy", async (AppDbContext context, DateTime date) =>
{
    var reportDate = DateOnly.FromDateTime(date);

    var bookingsOnDate = await context.Bookings
        .Include(b => b.Room)
        .Where(b => !b.IsDeleted
                    && b.Status == BookingStatus.CheckedIn.ToString()
                    && b.CheckInDate <= reportDate
                    && b.CheckOutDate >= reportDate)
        .ToListAsync();

    var result = bookingsOnDate.Select(b => new
    {
        b.BookingId,
        RoomNumber = b.Room!.RoomNumber,
        RoomType = b.Room!.RoomType,
        GuestId = b.GuestId,
        CheckInDate = b.CheckInDate,
        CheckOutDate = b.CheckOutDate,
        Status = b.Status
    });

    return Results.Ok(new
    {
        Date = reportDate,
        TotalOccupiedRooms = bookingsOnDate.Count,
        Bookings = result
    });
});

#endregion

app.Run();