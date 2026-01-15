using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MeetingRoomBooking.Api.Tests;

public class ApiSmokeTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ApiSmokeTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_Health_ShouldReturn200()
    {
        // 如果你没有 /api/health，就先用 swagger
        var resp = await _client.GetAsync("/swagger/index.html");

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }
}
