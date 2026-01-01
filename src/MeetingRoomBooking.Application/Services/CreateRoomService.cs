using MeetingRoomBooking.Application.Dtos;
using MeetingRoomBooking.Application.Interfaces;
using MeetingRoomBooking.Domain;

namespace MeetingRoomBooking.Application.Services;

public class CreateRoomService
{
    private readonly IRoomRepository _roomRepository;

    public CreateRoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<CreateRoomResult> CreateAsync(CreateRoomRequest request)
    {
        // 1) 基本输入校验（应用层）
        if (string.IsNullOrWhiteSpace(request.Name))
            return new CreateRoomResult(false, "Room name cannot be empty.", null);

        if (request.Capacity <= 0)
            return new CreateRoomResult(false, "Capacity must be greater than zero.", null);

        // 2) 业务规则：同名不允许重复
        var existing = await _roomRepository.GetByNameAsync(request.Name.Trim());
        if (existing is not null)
            return new CreateRoomResult(false, "Room with the same name already exists.", null);

        // 3) 构造 Domain 对象（触发 Domain 不变式）
        Room room;
        try
        {
            room = new Room(Guid.NewGuid(), request.Name.Trim(), request.Capacity, true);
        }
        catch (ArgumentException ex)
        {
            return new CreateRoomResult(false, ex.Message, null);
        }

        // 4) 保存
        await _roomRepository.AddAsync(room);

        return new CreateRoomResult(true, null, room.Id);
    }
}
    