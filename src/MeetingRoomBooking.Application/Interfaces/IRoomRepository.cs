using MeetingRoomBooking.Domain;

namespace MeetingRoomBooking.Application.Interfaces;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(Guid roomId);

    Task<IReadOnlyList<Room>> GetAllAsync();

    
    Task<Room?> GetByNameAsync(string name);
    Task AddAsync(Room room);
}
