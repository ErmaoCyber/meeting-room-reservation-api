namespace MeetingRoomBooking.Application.Dtos;

public record CreateBookingRequest(
    Guid RoomId,
    DateTime StartTime,
    DateTime EndTime
);
