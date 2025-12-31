namespace MeetingRoomBooking.Application.Dtos;

public record CreateBookingResult(
    bool Success,
    string? Error,
    Guid? BookingId
);
