namespace MeetingRoomBooking.Application.Dtos;

public record BookingDetailDto(Guid Id, Guid RoomId, DateTime StartTime, DateTime EndTime);
