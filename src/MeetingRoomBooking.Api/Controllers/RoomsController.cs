using MeetingRoomBooking.Application.Dtos;
using MeetingRoomBooking.Application.Services;
using Microsoft.AspNetCore.Mvc;
using MeetingRoomBooking.Api.Dtos;


namespace MeetingRoomBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly GetRoomsService _getRoomsService;
    private readonly GetRoomByIdService _getRoomByIdService;
    private readonly CreateRoomService _createRoomService;

    public RoomsController(
        GetRoomsService getRoomsService,
        GetRoomByIdService getRoomByIdService,
        CreateRoomService createRoomService)
    {
        _getRoomsService = getRoomsService;
        _getRoomByIdService = getRoomByIdService;
        _createRoomService = createRoomService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var rooms = await _getRoomsService.GetAsync();
        return Ok(rooms);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var room = await _getRoomByIdService.GetAsync(id);
        if (room is null)
            return Error(StatusCodes.Status404NotFound, "RoomNotFound", "Room not found.");


        return Ok(room);
    }

    private string? TraceId => HttpContext?.TraceIdentifier;

    private IActionResult Error(int statusCode, string errorCode, string message)
    {
        return StatusCode(statusCode, new ApiErrorResponse(errorCode, message, TraceId));
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomRequest request)
    {
        var result = await _createRoomService.CreateAsync(request);

        if (!result.Success)
            return Error(StatusCodes.Status400BadRequest, "InvalidRequest", result.ErrorMessage);


        return CreatedAtAction(nameof(GetById), new { id = result.RoomId }, new { roomId = result.RoomId });
    }

    // [HttpGet("throw")]
    // public IActionResult Throw()
    // {
    //     throw new InvalidOperationException("Test exception from /api/rooms/throw");
    // }

}
