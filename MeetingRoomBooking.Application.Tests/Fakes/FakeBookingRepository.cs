using MeetingRoomBooking.Application.Interfaces;
using MeetingRoomBooking.Domain;

namespace MeetingRoomBooking.Application.Tests.Fakes;

public class FakeBookingRepository : IBookingRepository
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

    public Task<Booking?> GetByIdAsync(Guid bookingId)
    {
        var booking = _bookings.SingleOrDefault(b => b.Id == bookingId);
        return Task.FromResult(booking);
    }
}
