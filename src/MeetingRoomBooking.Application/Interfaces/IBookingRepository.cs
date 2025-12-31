using MeetingRoomBooking.Domain;

namespace MeetingRoomBooking.Application.Interfaces;

public interface IBookingRepository
{
    Task<IReadOnlyList<Booking>> GetBookingsForRoomAsync(Guid roomId);
    Task AddAsync(Booking booking);
}
