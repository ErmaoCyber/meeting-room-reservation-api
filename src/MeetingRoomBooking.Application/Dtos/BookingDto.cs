namespace MeetingRoomBooking.Application.Dtos;

public record BookingDto(
    Guid Id,
    Guid RoomId,
    DateTime StartTime,
    DateTime EndTime
);
