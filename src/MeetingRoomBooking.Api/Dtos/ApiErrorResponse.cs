namespace MeetingRoomBooking.Api.Dtos;

public record ApiErrorResponse(string ErrorCode, string Message, string? TraceId);
