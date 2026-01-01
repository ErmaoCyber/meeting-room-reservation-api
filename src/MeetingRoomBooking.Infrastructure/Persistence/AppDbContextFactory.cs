using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MeetingRoomBooking.Infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // 设计时使用 SQLite（路径随便，迁移只关心模型）
        optionsBuilder.UseSqlite("Data Source=meetingroombooking.db");

        return new AppDbContext(optionsBuilder.Options);
    }
}
