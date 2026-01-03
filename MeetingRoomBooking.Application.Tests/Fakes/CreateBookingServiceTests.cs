using MeetingRoomBooking.Application.Dtos;
using MeetingRoomBooking.Application.Services;
using MeetingRoomBooking.Application.Tests.Fakes;
using MeetingRoomBooking.Domain;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace MeetingRoomBooking.Application.Tests;

public class CreateBookingServiceTests
{
    [Fact]
    public async Task CreateAsync_WhenRoomNotFound_ReturnsRoomNotFound()
    {
        var bookingRepo = new FakeBookingRepository();
        var roomRepo = new FakeRoomRepository(seed: Array.Empty<Room>());

        var logger = NullLogger<CreateBookingService>.Instance;
        var service = new CreateBookingService(bookingRepo, roomRepo, logger);

        var req = new CreateBookingRequest(
            Guid.NewGuid(),
            new DateTime(2026, 1, 1, 9, 0, 0),
            new DateTime(2026, 1, 1, 10, 0, 0)
        );

        var result = await service.CreateAsync(req);

        Assert.False(result.Success);
        Assert.Equal(BookingErrorCode.RoomNotFound, result.ErrorCode);
    }

    [Fact]
    public async Task CreateAsync_WhenRoomInactive_ReturnsRoomInactive()
    {
        var roomId = Guid.NewGuid();
        var bookingRepo = new FakeBookingRepository();
        var roomRepo = new FakeRoomRepository(seed: new[]
        {
            new Room(roomId, "Room X", 8, isActive: false)
        });

        var logger = NullLogger<CreateBookingService>.Instance;
        var service = new CreateBookingService(bookingRepo, roomRepo, logger);

        var req = new CreateBookingRequest(
            roomId,
            new DateTime(2026, 1, 1, 9, 0, 0),
            new DateTime(2026, 1, 1, 10, 0, 0)
        );

        var result = await service.CreateAsync(req);

        Assert.False(result.Success);
        Assert.Equal(BookingErrorCode.RoomInactive, result.ErrorCode);
    }

    [Fact]
    public async Task CreateAsync_WhenInvalidTimeRange_ReturnsInvalidRequest()
    {
        var roomId = Guid.NewGuid();
        var bookingRepo = new FakeBookingRepository();
        var roomRepo = new FakeRoomRepository(seed: new[]
        {
            new Room(roomId, "Room A", 8, isActive: true)
        });

        var logger = NullLogger<CreateBookingService>.Instance;
        var service = new CreateBookingService(bookingRepo, roomRepo, logger);

        // end <= start
        var req = new CreateBookingRequest(
            roomId,
            new DateTime(2026, 1, 1, 10, 0, 0),
            new DateTime(2026, 1, 1, 9, 0, 0)
        );

        var result = await service.CreateAsync(req);

        Assert.False(result.Success);
        Assert.Equal(BookingErrorCode.InvalidRequest, result.ErrorCode);
    }

    [Fact]
    public async Task CreateAsync_WhenConflict_ReturnsBookingConflict()
    {
        var roomId = Guid.NewGuid();
        var bookingRepo = new FakeBookingRepository();
        var roomRepo = new FakeRoomRepository(seed: new[]
        {
            new Room(roomId, "Room A", 8, isActive: true)
        });

        // 先塞一个已有预订
        await bookingRepo.AddAsync(new Booking(
            Guid.NewGuid(),
            roomId,
            new DateTime(2026, 1, 1, 9, 0, 0),
            new DateTime(2026, 1, 1, 10, 0, 0)
        ));

        var logger = NullLogger<CreateBookingService>.Instance;
        var service = new CreateBookingService(bookingRepo, roomRepo, logger);

        var req = new CreateBookingRequest(
            roomId,
            new DateTime(2026, 1, 1, 9, 30, 0),
            new DateTime(2026, 1, 1, 10, 30, 0)
        );

        var result = await service.CreateAsync(req);

        Assert.False(result.Success);
        Assert.Equal(BookingErrorCode.BookingConflict, result.ErrorCode);
    }

    [Fact]
    public async Task CreateAsync_WhenValid_ReturnsSuccessAndBookingId()
    {
        var roomId = Guid.NewGuid();
        var bookingRepo = new FakeBookingRepository();
        var roomRepo = new FakeRoomRepository(seed: new[]
        {
            new Room(roomId, "Room A", 8, isActive: true)
        });

        var logger = NullLogger<CreateBookingService>.Instance;
        var service = new CreateBookingService(bookingRepo, roomRepo, logger);

        var req = new CreateBookingRequest(
            roomId,
            new DateTime(2026, 1, 1, 9, 0, 0),
            new DateTime(2026, 1, 1, 10, 0, 0)
        );

        var result = await service.CreateAsync(req);

        Assert.True(result.Success);
        Assert.NotNull(result.BookingId);
        Assert.Equal(BookingErrorCode.None, result.ErrorCode);
    }
}
