namespace MeetingRoomBooking.Application.Dtos;

public record CreateRoomResult(bool Success, string? ErrorMessage, Guid? RoomId);
