using MeetingRoomBooking.Application.Interfaces;
using MeetingRoomBooking.Domain;

namespace MeetingRoomBooking.Application.Tests.Fakes;

public class FakeRoomRepository : IRoomRepository
{
    private readonly List<Room> _rooms;

    public FakeRoomRepository(IEnumerable<Room>? seed = null)
    {
        _rooms = seed?.ToList() ?? new List<Room>();
    }

    public Task<Room?> GetByIdAsync(Guid roomId)
    {
        var room = _rooms.SingleOrDefault(r => r.Id == roomId);
        return Task.FromResult(room);
    }

    public Task<IReadOnlyList<Room>> GetAllAsync()
    {
        IReadOnlyList<Room> result = _rooms.ToList();
        return Task.FromResult(result);
    }

    public Task<Room?> GetByNameAsync(string name)
    {
        var room = _rooms.SingleOrDefault(r =>
            string.Equals(r.Name, name, StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(room);
    }

    public Task AddAsync(Room room)
    {
        _rooms.Add(room);
        return Task.CompletedTask;
    }
}
