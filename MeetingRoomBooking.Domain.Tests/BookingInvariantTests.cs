using MeetingRoomBooking.Domain;
using Xunit;

namespace MeetingRoomBooking.Domain.Tests;

public class BookingInvariantTests
{
    [Fact]
    public void Constructor_WhenEndBeforeStart_ThrowsArgumentException()
    {
        var roomId = Guid.NewGuid();

        Assert.Throws<ArgumentException>(() =>
            new Booking(Guid.NewGuid(), roomId,
                new DateTime(2026, 1, 1, 10, 0, 0),
                new DateTime(2026, 1, 1, 9, 0, 0)));
    }
}
