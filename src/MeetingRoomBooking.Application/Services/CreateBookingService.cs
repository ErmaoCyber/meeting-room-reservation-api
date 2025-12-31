using MeetingRoomBooking.Application.Dtos;
using MeetingRoomBooking.Application.Interfaces;
using MeetingRoomBooking.Domain;

namespace MeetingRoomBooking.Application.Services;

public class CreateBookingService
{
    private readonly IBookingRepository _bookingRepository;

    public CreateBookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<CreateBookingResult> CreateAsync(CreateBookingRequest request)
    {
        // 1) 构造 Domain 对象（这里会触发基本不变式校验，如 End > Start）
        Booking newBooking;
        try
        {
            newBooking = new Booking(
                id: Guid.NewGuid(),
                roomId: request.RoomId,
                startTime: request.StartTime,
                endTime: request.EndTime
            );
        }
        catch (ArgumentException ex)
        {
            return new CreateBookingResult(false, ex.Message, null);
        }

        // 2) 读取该房间已有预订
        var existing = await _bookingRepository.GetBookingsForRoomAsync(request.RoomId);

        // 3) 冲突检测（Domain Service）
        if (BookingConflictService.HasConflict(newBooking, existing))
        {
            return new CreateBookingResult(false, "Booking conflicts with an existing booking.", null);
        }

        // 4) 保存
        await _bookingRepository.AddAsync(newBooking);

        return new CreateBookingResult(true, null, newBooking.Id);
    }
}
