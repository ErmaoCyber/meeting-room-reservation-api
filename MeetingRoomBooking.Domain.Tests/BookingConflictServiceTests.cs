using MeetingRoomBooking.Domain;
using Xunit;

namespace MeetingRoomBooking.Domain.Tests;

public class BookingConflictServiceTests
{
    private static Booking NewBooking(Guid roomId, DateTime start, DateTime end)
        => new Booking(Guid.NewGuid(), roomId, start, end);

    [Fact]
    public void HasConflict_WhenNoExistingBookings_ReturnsFalse()
    {
        var roomId = Guid.NewGuid();
        var newBooking = NewBooking(roomId,
            new DateTime(2026, 1, 1, 9, 0, 0),
            new DateTime(2026, 1, 1, 10, 0, 0));

        var existing = Array.Empty<Booking>();

        var result = BookingConflictService.HasConflict(newBooking, existing);

        Assert.False(result);
    }

    [Fact]
    public void HasConflict_WhenTimesOverlap_ReturnsTrue()
    {
        var roomId = Guid.NewGuid();

        var existing = new[]
        {
            NewBooking(roomId,
                new DateTime(2026, 1, 1, 9, 0, 0),
                new DateTime(2026, 1, 1, 10, 0, 0))
        };

        var newBooking = NewBooking(roomId,
            new DateTime(2026, 1, 1, 9, 30, 0),
            new DateTime(2026, 1, 1, 10, 30, 0));

        var result = BookingConflictService.HasConflict(newBooking, existing);

        Assert.True(result);
    }

    [Fact]
    public void HasConflict_WhenBackToBack_DoesNotConflict()
    {
        var roomId = Guid.NewGuid();

        var existing = new[]
        {
            NewBooking(roomId,
                new DateTime(2026, 1, 1, 9, 0, 0),
                new DateTime(2026, 1, 1, 10, 0, 0))
        };

        // 工程约定：允许“前一场结束时间 == 下一场开始时间”（半开区间 [start, end)）
        var newBooking = NewBooking(roomId,
            new DateTime(2026, 1, 1, 10, 0, 0),
            new DateTime(2026, 1, 1, 11, 0, 0));

        var result = BookingConflictService.HasConflict(newBooking, existing);

        Assert.False(result);
    }
}
