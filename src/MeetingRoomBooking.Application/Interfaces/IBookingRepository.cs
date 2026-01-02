using MeetingRoomBooking.Domain;

namespace MeetingRoomBooking.Application.Interfaces;

public interface IBookingRepository
{
    Task<IReadOnlyList<Booking>> GetBookingsForRoomAsync(Guid roomId);

    Task<IReadOnlyList<Booking>> GetAllForRoomAsync(Guid roomId);

    Task AddAsync(Booking booking);

    Task<Booking?> GetByIdAsync(Guid bookingId);

}
