using MeetingRoomBooking.Application.Dtos;
using MeetingRoomBooking.Application.Interfaces;

namespace MeetingRoomBooking.Application.Services;

public class GetRoomsService
{
    private readonly IRoomRepository _roomRepository;

    public GetRoomsService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<IReadOnlyList<RoomDto>> GetAsync()
    {
        var rooms = await _roomRepository.GetAllAsync();

        return rooms
            .Select(r => new RoomDto(r.Id, r.Name, r.Capacity, r.IsActive))
            .ToList();
    }
}
