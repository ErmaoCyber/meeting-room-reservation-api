namespace MeetingRoomBooking.Domain;

public class Booking
{
    public Guid Id { get; }
    public Guid RoomId { get; }
    public DateTime StartTime { get; }
    public DateTime EndTime { get; }

    public Booking(Guid id, Guid roomId, DateTime startTime, DateTime endTime)
    {
        if (endTime <= startTime)
            throw new ArgumentException("End time must be later than start time.");

        Id = id;
        RoomId = roomId;
        StartTime = startTime;
        EndTime = endTime;
    }
}
