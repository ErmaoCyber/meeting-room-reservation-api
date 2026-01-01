using MeetingRoomBooking.Application.Interfaces;
using MeetingRoomBooking.Domain;

namespace MeetingRoomBooking.Infrastructure;

public class InMemoryRoomRepository : IRoomRepository
{
    private readonly List<Room> _rooms = new()
    {
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
}
