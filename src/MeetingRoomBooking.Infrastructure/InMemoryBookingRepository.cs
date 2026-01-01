using MeetingRoomBooking.Application.Interfaces;
using MeetingRoomBooking.Domain;

namespace MeetingRoomBooking.Infrastructure;

public class InMemoryBookingRepository : IBookingRepository
{
    private readonly List<Booking> _bookings = new();

    public Task<IReadOnlyList<Booking>> GetBookingsForRoomAsync(Guid roomId)
    {
        IReadOnlyList<Booking> result = _bookings
            .Where(b => b.RoomId == roomId)
            .ToList();

        return Task.FromResult(result);
    }

    public Task<IReadOnlyList<Booking>> GetAllForRoomAsync(Guid roomId)
    {
        IReadOnlyList<Booking> result = _bookings
            .Where(b => b.RoomId == roomId)
            .OrderBy(b => b.StartTime)
            .ToList();

        return Task.FromResult(result);
    }

    public Task AddAsync(Booking booking)
    {
        _bookings.Add(booking);
        return Task.CompletedTask;
    }
}
