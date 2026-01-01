namespace MeetingRoomBooking.Application.Dtos;

public enum BookingErrorCode
{
    None = 0,
    InvalidRequest = 1,
    RoomNotFound = 2,
    RoomInactive = 3,
    BookingConflict = 4
}
