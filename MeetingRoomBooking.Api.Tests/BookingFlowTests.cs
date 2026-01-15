using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MeetingRoomBooking.Api.Tests;

public class BookingFlowTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BookingFlowTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateRoom_ThenCreateBooking_ThenGetBookings_ShouldWork()
    {
        // 关键：用唯一名字，保证测试可重复运行（避免撞 “same name already exists”）
        var uniqueRoomName = $"Test Room {Guid.NewGuid():N}";

        // 1) Create Room
        var createRoomReq = new
        {
            Name = uniqueRoomName,
            Capacity = 6
        };

        var roomResp = await _client.PostAsJsonAsync("/api/rooms", createRoomReq);
        if (roomResp.StatusCode != HttpStatusCode.Created)
        {
            var body = await roomResp.Content.ReadAsStringAsync();
            throw new Exception($"CreateRoom failed: {(int)roomResp.StatusCode} {roomResp.StatusCode}\n{body}");
        }

        var roomJson = await roomResp.Content.ReadFromJsonAsync<JsonElement>();
        var roomId = ExtractGuid(roomJson, "roomId", "id", "RoomId", "Id");

        // 2) Create Booking
        var createBookingReq = new
        {
            RoomId = roomId,
            StartTime = new DateTime(2026, 1, 1, 9, 0, 0),
            EndTime = new DateTime(2026, 1, 1, 10, 0, 0)
        };

        var bookingResp = await _client.PostAsJsonAsync("/api/bookings", createBookingReq);
        if (bookingResp.StatusCode != HttpStatusCode.Created)
        {
            var body = await bookingResp.Content.ReadAsStringAsync();
            throw new Exception($"CreateBooking failed: {(int)bookingResp.StatusCode} {bookingResp.StatusCode}\n{body}");
        }

        // 3) Get bookings for this room
        var listResp = await _client.GetAsync($"/api/bookings?roomId={roomId}");
        if (listResp.StatusCode != HttpStatusCode.OK)
        {
            var body = await listResp.Content.ReadAsStringAsync();
            throw new Exception($"GetBookings failed: {(int)listResp.StatusCode} {listResp.StatusCode}\n{body}");
        }

        var listJson = await listResp.Content.ReadFromJsonAsync<JsonElement>();
        if (listJson.ValueKind != JsonValueKind.Array)
        {
            throw new Exception($"Expected array but got {listJson.ValueKind}: {listJson.GetRawText()}");
        }

        Assert.True(listJson.GetArrayLength() >= 1);
    }

    private static Guid ExtractGuid(JsonElement json, params string[] candidatePropertyNames)
    {
        if (json.ValueKind != JsonValueKind.Object)
            throw new Exception($"Expected JSON object but got {json.ValueKind}: {json.GetRawText()}");

        foreach (var name in candidatePropertyNames)
        {
            if (!json.TryGetProperty(name, out var prop))
                continue;

            if (prop.ValueKind == JsonValueKind.String && Guid.TryParse(prop.GetString(), out var g))
                return g;
        }

        throw new Exception($"Could not find a GUID in response JSON: {json.GetRawText()}");
    }
}
