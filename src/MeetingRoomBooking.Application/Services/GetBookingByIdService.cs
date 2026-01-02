using MeetingRoomBooking.Application.Dtos;
using MeetingRoomBooking.Application.Interfaces;

namespace MeetingRoomBooking.Application.Services;

public class GetBookingByIdService
{
    private readonly IBookingRepository _bookingRepository;

    public GetBookingByIdService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<BookingDetailDto?> GetAsync(Guid bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);
        if (booking is null) return null;

        return new BookingDetailDto(booking.Id, booking.RoomId, booking.StartTime, booking.EndTime);
    }
}
