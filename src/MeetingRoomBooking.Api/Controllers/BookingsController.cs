using Microsoft.AspNetCore.Mvc;
using MeetingRoomBooking.Application.Dtos;
using MeetingRoomBooking.Application.Services;

namespace MeetingRoomBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly CreateBookingService _service;

    public BookingsController(CreateBookingService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookingRequest request)
    {
        var result = await _service.CreateAsync(request);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}
