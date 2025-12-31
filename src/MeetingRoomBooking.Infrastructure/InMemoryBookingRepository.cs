using MeetingRoomBooking.Application.Interfaces;
using MeetingRoomBooking.Domain;

namespace MeetingRoomBooking.Infrastructure;

public class InMemoryBookingRepository : IBookingRepository
{
    // 简单起见：先用 List 存。后面换数据库也不影响 Application 代码。
    private readonly List<Booking> _bookings = new();

    public Task<IReadOnlyList<Booking>> GetBookingsForRoomAsync(Guid roomId)
    {
        IReadOnlyList<Booking> result = _bookings
            .Where(b => b.RoomId == roomId)
            .ToList();

        return Task.FromResult(result);
    }

    public Task AddAsync(Booking booking)
    {
        _bookings.Add(booking);
        return Task.CompletedTask;
    }
}
