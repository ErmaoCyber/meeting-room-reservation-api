using MeetingRoomBooking.Application.Dtos;
using MeetingRoomBooking.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly GetRoomsService _getRoomsService;
    private readonly CreateRoomService _createRoomService;

    public RoomsController(GetRoomsService getRoomsService, CreateRoomService createRoomService)
    {
        _getRoomsService = getRoomsService;
        _createRoomService = createRoomService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var rooms = await _getRoomsService.GetAsync();
        return Ok(rooms);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoomRequest request)
    {
        var result = await _createRoomService.CreateAsync(request);

        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        // 201 Created + Location
        return CreatedAtAction(nameof(Get), new { id = result.RoomId }, new { roomId = result.RoomId });
    }
}
