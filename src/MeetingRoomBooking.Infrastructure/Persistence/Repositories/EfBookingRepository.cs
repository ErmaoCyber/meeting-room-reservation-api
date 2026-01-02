using MeetingRoomBooking.Application.Interfaces;
using MeetingRoomBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomBooking.Infrastructure.Persistence.Repositories;

public class EfBookingRepository : IBookingRepository
{
    private readonly AppDbContext _db;

    public EfBookingRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Booking>> GetBookingsForRoomAsync(Guid roomId)
    {
        return await _db.Bookings
            .Where(b => b.RoomId == roomId)
            .OrderBy(b => b.StartTime)
            .ToListAsync();
    }

    public async Task AddAsync(Booking booking)
    {
        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Booking>> GetAllForRoomAsync(Guid roomId)
    {
        return await _db.Bookings
            .Where(b => b.RoomId == roomId)
            .OrderBy(b => b.StartTime)
            .ToListAsync();
    }

    public async Task<Booking?> GetByIdAsync(Guid bookingId)
    {
        return await _db.Bookings.SingleOrDefaultAsync(b => b.Id == bookingId);
    }

}
