using MeetingRoomBooking.Application.Dtos;
using MeetingRoomBooking.Application.Interfaces;

namespace MeetingRoomBooking.Application.Services;

public class GetBookingsForRoomService
{
    private readonly IBookingRepository _repository;

    public GetBookingsForRoomService(IBookingRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<BookingDto>> GetAsync(Guid roomId)
    {
        var bookings = await _repository.GetAllForRoomAsync(roomId);

        return bookings
            .Select(b => new BookingDto(
                b.Id,
                b.RoomId,
                b.StartTime,
                b.EndTime))
            .ToList();
    }
}
