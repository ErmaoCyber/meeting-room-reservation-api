using MeetingRoomBooking.Application.Dtos;
using MeetingRoomBooking.Application.Interfaces;
using MeetingRoomBooking.Domain;
using Microsoft.Extensions.Logging;

namespace MeetingRoomBooking.Application.Services;

public class CreateBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly ILogger<CreateBookingService> _logger;

    // ✅ 只保留一个构造函数，并把 logger 注入进来
    public CreateBookingService(
        IBookingRepository bookingRepository,
        IRoomRepository roomRepository,
        ILogger<CreateBookingService> logger)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _logger = logger;
    }

    public async Task<CreateBookingResult> CreateAsync(CreateBookingRequest request)
    {

        _logger.LogInformation(
    "Create booking request RoomId={RoomId} Start={Start} End={End}",
    request.RoomId, request.StartTime, request.EndTime);

        var room = await _roomRepository.GetByIdAsync(request.RoomId);
        if (room is null)
        {
            _logger.LogWarning("Room not found. RoomId={RoomId}", request.RoomId);
            return new CreateBookingResult(false, BookingErrorCode.RoomNotFound, "Room not found.", null);
        }

        if (!room.IsActive)
        {
            _logger.LogWarning("Room inactive. RoomId={RoomId}", request.RoomId);
            return new CreateBookingResult(false, BookingErrorCode.RoomInactive, "Room is inactive.", null);
        }

        Booking newBooking;
        try
        {
            newBooking = new Booking(Guid.NewGuid(), request.RoomId, request.StartTime, request.EndTime);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid booking request.");
            return new CreateBookingResult(false, BookingErrorCode.InvalidRequest, ex.Message, null);
        }

        var existing = await _bookingRepository.GetBookingsForRoomAsync(request.RoomId);
        if (BookingConflictService.HasConflict(newBooking, existing))
        {
            _logger.LogInformation("Booking conflict detected. RoomId={RoomId}", request.RoomId);
            return new CreateBookingResult(false, BookingErrorCode.BookingConflict, "Booking conflicts with an existing booking.", null);
        }

        await _bookingRepository.AddAsync(newBooking);

        _logger.LogInformation("Booking created successfully. BookingId={BookingId}", newBooking.Id);
        return new CreateBookingResult(true, BookingErrorCode.None, null, newBooking.Id);

        // // ✅ 1) 入口日志（先记录意图）
        // _logger.LogInformation(
        //     "CreateBooking requested. RoomId={RoomId}, Start={Start}, End={End}",
        //     request.RoomId,
        //     request.StartTime,
        //     request.EndTime);

        // // ✅ 2) Room 存在性/可用性校验
        // var room = await _roomRepository.GetByIdAsync(request.RoomId);

        // if (room is null)
        // {
        //     _logger.LogWarning("CreateBooking failed: room not found. RoomId={RoomId}", request.RoomId);
        //     return new CreateBookingResult(false, BookingErrorCode.RoomNotFound, "Room not found.", null);
        // }

        // if (!room.IsActive)
        // {
        //     _logger.LogWarning("CreateBooking failed: room inactive. RoomId={RoomId}", request.RoomId);
        //     return new CreateBookingResult(false, BookingErrorCode.RoomInactive, "Room is inactive.", null);
        // }

        // // ✅ 3) 构造 Domain 对象（不变式校验，如 End > Start）
        // Booking newBooking;
        // try
        // {
        //     newBooking = new Booking(
        //         id: Guid.NewGuid(),
        //         roomId: request.RoomId,
        //         startTime: request.StartTime,
        //         endTime: request.EndTime
        //     );
        // }
        // catch (ArgumentException ex)
        // {
        //     _logger.LogWarning(
        //         ex,
        //         "CreateBooking failed: invalid request. RoomId={RoomId}",
        //         request.RoomId);

        //     return new CreateBookingResult(false, BookingErrorCode.InvalidRequest, ex.Message, null);
        // }

        // // ✅ 4) 冲突检测
        // var existing = await _bookingRepository.GetBookingsForRoomAsync(request.RoomId);

        // if (BookingConflictService.HasConflict(newBooking, existing))
        // {
        //     _logger.LogWarning("CreateBooking failed: booking conflict. RoomId={RoomId}", request.RoomId);
        //     return new CreateBookingResult(false, BookingErrorCode.BookingConflict, "Booking conflicts with an existing booking.", null);
        // }

        // // ✅ 5) 保存 + 成功日志
        // await _bookingRepository.AddAsync(newBooking);

        // _logger.LogInformation(
        //     "Booking created successfully. BookingId={BookingId}, RoomId={RoomId}",
        //     newBooking.Id,
        //     request.RoomId);

        // return new CreateBookingResult(true, BookingErrorCode.None, null, newBooking.Id);
    }
}
