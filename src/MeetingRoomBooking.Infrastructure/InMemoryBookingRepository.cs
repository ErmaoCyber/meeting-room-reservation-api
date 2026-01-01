using MeetingRoomBooking.Application.Interfaces;
using MeetingRoomBooking.Domain;

namespace MeetingRoomBooking.Infrastructure;

public class InMemoryBookingRepository : IBookingRepository
{
    // 简单起见：先用 List 存。后面换数据库也不影响 Application 代码。
    private readonly List<Booking> _bookings = new();
    private readonly List<Room> _rooms = new()

     {
        // 先内置一个“已存在”的房间，方便验收
        new Room(
            id: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            name: "Room A",
            capacity: 8,
            isActive: true)
    };

    public Task<Room?> GetByIdAsync(Guid roomId)
    {
        var room = _rooms.SingleOrDefault(r => r.Id == roomId);
        return Task.FromResult(room);
    }
    
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
