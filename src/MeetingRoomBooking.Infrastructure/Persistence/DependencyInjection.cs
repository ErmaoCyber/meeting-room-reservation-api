using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MeetingRoomBooking.Application.Interfaces;
using MeetingRoomBooking.Infrastructure.Persistence.Repositories;


namespace MeetingRoomBooking.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructurePersistence(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IBookingRepository, EfBookingRepository>();
        services.AddScoped<IRoomRepository, EfRoomRepository>();

        return services;
    }
}
