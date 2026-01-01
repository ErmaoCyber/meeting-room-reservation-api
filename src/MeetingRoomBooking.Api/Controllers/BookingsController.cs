using Microsoft.AspNetCore.Mvc;
using MeetingRoomBooking.Application.Dtos;
using MeetingRoomBooking.Application.Services;

namespace MeetingRoomBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    // 依赖 1：创建 Booking 的用例（Application 层）
    private readonly CreateBookingService _createBookingService;

    // 依赖 2：按 Room 查询 Booking 列表的用例（Application 层）
    private readonly GetBookingsForRoomService _getBookingsForRoomService;

    // 构造器注入：DI 会自动把 Program.cs 注册的服务传进来
    public BookingsController(
        CreateBookingService createBookingService,
        GetBookingsForRoomService getBookingsForRoomService)
    {
        _createBookingService = createBookingService;
        _getBookingsForRoomService = getBookingsForRoomService;
    }

    // 将业务错误码映射为正确的 HTTP 状态码（工程化：集中管理）
    private static int MapToStatusCode(BookingErrorCode code) => code switch
    {
        BookingErrorCode.InvalidRequest  => StatusCodes.Status400BadRequest,
        BookingErrorCode.RoomNotFound    => StatusCodes.Status404NotFound,
        BookingErrorCode.RoomInactive    => StatusCodes.Status409Conflict,
        BookingErrorCode.BookingConflict => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status400BadRequest
    };

    /// <summary>
    /// Create a booking for a room.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateBookingRequest request)
    {
        // 1) 调用 Application 层用例
        var result = await _createBookingService.CreateAsync(request);

        // 2) 失败：根据 ErrorCode 返回语义化状态码（400/404/409）
        if (!result.Success)
        {
            var status = MapToStatusCode(result.ErrorCode);

            // ⚠️ 这里用 result.ErrorMessage（如果你类里叫别的名字，请保持一致）
            return StatusCode(status, new
            {
                code = result.ErrorCode.ToString(),
                message = result.Error
            });
        }

        // 3) 成功：201 Created，并把 bookingId 返回给调用方
        // 当前你只有 “按 roomId 查询列表” 的 GET，所以 Location 指向 Get(roomId) 是合理的
        return CreatedAtAction(
            nameof(Get),
            new { roomId = request.RoomId },
            new { bookingId = result.BookingId }
        );
    }

    /// <summary>
    /// Get bookings for a room.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] Guid roomId)
    {
        var result = await _getBookingsForRoomService.GetAsync(roomId);
        return Ok(result);
    }
}
