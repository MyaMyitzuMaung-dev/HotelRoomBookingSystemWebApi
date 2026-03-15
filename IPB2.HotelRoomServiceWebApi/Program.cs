using IPB2.HotelRoomService.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Services
builder.Services.AddScoped<IPB2.HotelRoomServiceWebApi.Features.Guest.GuestService>();
builder.Services.AddScoped<IPB2.HotelRoomServiceWebApi.Features.Booking.BookingService>();
builder.Services.AddScoped<IPB2.HotelRoomServiceWebApi.Features.Room.RoomService>();
builder.Services.AddScoped<IPB2.HotelRoomServiceWebApi.Features.Report.ReportService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
