namespace MeetingRoomBooking.Domain;

public static class BookingConflictService
{
    public static bool HasConflict(
        Booking newBooking,
        IEnumerable<Booking> existingBookings)
    {
        foreach (var existing in existingBookings)
        {
            if (IsOverlapping(newBooking, existing))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsOverlapping(Booking a, Booking b)
    {
        return a.StartTime < b.EndTime && a.EndTime > b.StartTime;
    }
}
