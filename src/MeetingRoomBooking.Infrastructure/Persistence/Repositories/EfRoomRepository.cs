using MeetingRoomBooking.Application.Interfaces;
using MeetingRoomBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomBooking.Infrastructure.Persistence.Repositories;

public class EfRoomRepository : IRoomRepository
{
    private readonly AppDbContext _db;

    public EfRoomRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Room?> GetByIdAsync(Guid roomId)
    {
        return await _db.Rooms.SingleOrDefaultAsync(r => r.Id == roomId);
    }
}
