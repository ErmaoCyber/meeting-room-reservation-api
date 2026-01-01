using MeetingRoomBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomBooking.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var roomId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        modelBuilder.Entity<Room>().HasData(new
        {
            Id = roomId,
            Name = "Room A",
            Capacity = 8,
            IsActive = true
        });
        
        modelBuilder.Entity<Room>(entity =>
        {
            entity.ToTable("Rooms");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired();
            entity.Property(r => r.Capacity).IsRequired();
            entity.Property(r => r.IsActive).IsRequired();
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("Bookings");
            entity.HasKey(b => b.Id);
            entity.Property(b => b.RoomId).IsRequired();
            entity.Property(b => b.StartTime).IsRequired();
            entity.Property(b => b.EndTime).IsRequired();
            entity.HasIndex(b => new { b.RoomId, b.StartTime, b.EndTime });
        });
    }
}
