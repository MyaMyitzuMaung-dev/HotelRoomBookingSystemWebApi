using IPB2.HotelRoomService.Database.AppDbContextModels;
using IPB2.HotelRoomService.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace IPB2.HotelRoomServiceWebApi.Features.Booking
{
    public class BookingService
    {
        private readonly AppDbContext _context;

        public BookingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BookRoomResponse> BookRoomAsync(BookRoomRequest request)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomId == request.RoomId && !r.IsDeleted);

            if (room == null)
            {
                return new BookRoomResponse { IsSuccess = false, Message = "Room not found" };
            }

            var nights = request.CheckOutDate.DayNumber - request.CheckInDate.DayNumber;

            if (nights <= 0)
            {
                return new BookRoomResponse { IsSuccess = false, Message = "Invalid check-in and check-out date" };
            }

            decimal totalAmount = (room.PricePerNight ?? 0) * nights;

            var booking = new IPB2.HotelRoomService.Database.AppDbContextModels.Booking
            {
                RoomId = request.RoomId,
                GuestId = request.GuestId,
                BookingDate = DateTime.Now,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                TotalAmount = totalAmount,
                Status = BookingStatus.Booked.ToString(),
                IsDeleted = false
            };

            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            return new BookRoomResponse
            {
                IsSuccess = true,
                BookingId = booking.BookingId,
                Message = $"Room booked successfully. Total Amount = {totalAmount}"
            };
        }

        public async Task<GetBookingsResponse> GetBookingsAsync(GetBookingsRequest request)
        {
            var bookings = await _context.Bookings
                .Where(x => !x.IsDeleted)
                .Skip((request.PageNo - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new BookingItem
                {
                    BookingId = x.BookingId,
                    RoomId = x.RoomId,
                    GuestId = x.GuestId,
                    CheckInDate = (DateOnly)x.CheckInDate!,
                    CheckOutDate = (DateOnly)x.CheckOutDate!,
                    Status = Enum.Parse<BookingStatus>(x.Status!),
                    TotalAmount = (decimal)x.TotalAmount!
                })
                .ToListAsync();

            return new GetBookingsResponse
            {
                Data = bookings
            };
        }

        public async Task<CheckInResponse> CheckInAsync(CheckInRequest request)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(x => x.BookingId == request.BookingId && !x.IsDeleted);

            if (booking == null)
            {
                return new CheckInResponse { IsSuccess = false, Message = "Booking not found" };
            }

            booking.Status = BookingStatus.CheckedIn.ToString();
            await _context.SaveChangesAsync();

            return new CheckInResponse { IsSuccess = true, Message = "Check-in successful" };
        }

        public async Task<CheckOutResponse> CheckOutAsync(CheckOutRequest request)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(x => x.BookingId == request.BookingId && !x.IsDeleted);

            if (booking == null)
            {
                return new CheckOutResponse { IsSuccess = false, Message = "Booking not found" };
            }

            booking.Status = BookingStatus.CheckedOut.ToString();
            await _context.SaveChangesAsync();

            return new CheckOutResponse { IsSuccess = true, Message = "Check-out successful" };
        }

        public async Task<DeleteBookingResponse> DeleteBookingAsync(DeleteBookingRequest request)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(x => x.BookingId == request.BookingId);

            if (booking == null)
            {
                return new DeleteBookingResponse { IsSuccess = false, Message = "Booking not found" };
            }

            booking.IsDeleted = true;
            await _context.SaveChangesAsync();

            return new DeleteBookingResponse { IsSuccess = true, Message = "Booking deleted" };
        }

        public async Task<CancelBookingResponse> CancelBookingAsync(CancelBookingRequest request)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(x => x.BookingId == request.BookingId && !x.IsDeleted);

            if (booking == null)
            {
                return new CancelBookingResponse { IsSuccess = false, Message = "Booking not found" };
            }

            if (booking.Status == BookingStatus.CheckedIn.ToString())
            {
                return new CancelBookingResponse { IsSuccess = false, Message = "Cannot cancel after check-in" };
            }

            booking.Status = BookingStatus.Cancelled.ToString();
            await _context.SaveChangesAsync();

            return new CancelBookingResponse { IsSuccess = true, Message = "Booking cancelled successfully" };
        }
    }
}
