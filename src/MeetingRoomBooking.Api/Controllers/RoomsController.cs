using MeetingRoomBooking.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeetingRoomBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly GetRoomsService _getRoomsService;

    public RoomsController(GetRoomsService getRoomsService)
    {
        _getRoomsService = getRoomsService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var rooms = await _getRoomsService.GetAsync();
        return Ok(rooms);
    }
}
