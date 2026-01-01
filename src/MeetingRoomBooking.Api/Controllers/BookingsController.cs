using Microsoft.AspNetCore.Mvc;
using MeetingRoomBooking.Application.Dtos;
using MeetingRoomBooking.Application.Services;

namespace MeetingRoomBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly CreateBookingService _service;

    private readonly GetBookingsForRoomService _queryService;

    public BookingsController(CreateBookingService service, GetBookingsForRoomService queryService)
    {
        _service = service;
        _queryService = queryService;

    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateBookingResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CreateBookingResult), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CreateBookingResult), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(CreateBookingResult), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateBookingRequest request)
    {
        var result = await _service.CreateAsync(request);

        if (!result.Success)
        {
            return result.ErrorCode switch
            {
                BookingErrorCode.RoomNotFound => NotFound(result),
                BookingErrorCode.BookingConflict => Conflict(result),
                BookingErrorCode.RoomInactive => BadRequest(result),
                BookingErrorCode.InvalidRequest => BadRequest(result),
                _ => BadRequest(result)
            };
        }

        return Ok(result);

    }
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Guid roomId)
    {
        var result = await _queryService.GetAsync(roomId);
        return Ok(result);
    }

}
