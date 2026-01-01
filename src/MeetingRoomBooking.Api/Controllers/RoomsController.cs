using MeetingRoomBooking.Application.Dtos;
using MeetingRoomBooking.Application.Services;
using Microsoft.AspNetCore.Mvc;

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
        if (room is null) return NotFound(new { message = "Room not found." });

        return Ok(room);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomRequest request)
    {
        var result = await _createRoomService.CreateAsync(request);

        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        return CreatedAtAction(nameof(GetById), new { id = result.RoomId }, new { roomId = result.RoomId });
    }
}
