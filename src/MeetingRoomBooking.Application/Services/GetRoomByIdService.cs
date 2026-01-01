using MeetingRoomBooking.Application.Dtos;
using MeetingRoomBooking.Application.Interfaces;

namespace MeetingRoomBooking.Application.Services;

public class GetRoomByIdService
{
    private readonly IRoomRepository _roomRepository;

    public GetRoomByIdService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<RoomDetailDto?> GetAsync(Guid roomId)
    {
        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room is null) return null;

        return new RoomDetailDto(room.Id, room.Name, room.Capacity, room.IsActive);
    }
}
