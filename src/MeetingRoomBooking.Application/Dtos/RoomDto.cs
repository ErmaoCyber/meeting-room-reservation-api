namespace MeetingRoomBooking.Application.Dtos;

public record RoomDto(Guid Id, string Name, int Capacity, bool IsActive);
