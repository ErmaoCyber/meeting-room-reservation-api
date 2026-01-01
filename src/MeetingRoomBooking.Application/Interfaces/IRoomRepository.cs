using MeetingRoomBooking.Domain;

namespace MeetingRoomBooking.Application.Interfaces;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(Guid roomId);
}
