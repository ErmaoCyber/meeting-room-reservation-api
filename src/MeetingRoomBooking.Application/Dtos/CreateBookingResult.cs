namespace MeetingRoomBooking.Application.Dtos;

public record CreateBookingResult(
    bool Success,
    BookingErrorCode ErrorCode,
    string? Error,
    Guid? BookingId
);
