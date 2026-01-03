namespace MeetingRoomBooking.Api.Errors;

public record ApiErrorResponse(
    string Code,
    string Message,
    string TraceId
);
