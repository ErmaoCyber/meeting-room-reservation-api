namespace MeetingRoomBooking.Application.Dtos;

public record RoomDetailDto(Guid Id, string Name, int Capacity, bool IsActive);
