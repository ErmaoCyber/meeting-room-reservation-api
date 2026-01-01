using MeetingRoomBooking.Application.Services;
using MeetingRoomBooking.Application.Interfaces;
using MeetingRoomBooking.Infrastructure;
using MeetingRoomBooking.Infrastructure.Persistence;
using MeetingRoomBooking.Domain;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// builder.Services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();
builder.Services.AddScoped<CreateBookingService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<GetBookingsForRoomService>();
// builder.Services.AddSingleton<IRoomRepository, InMemoryRoomRepository>();
builder.Services.AddInfrastructurePersistence("Data Source=meetingroombooking.db");





var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    var roomId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    if (!db.Rooms.Any(r => r.Id == roomId))
    {
        db.Rooms.Add(new Room(roomId, "Room A", 8, true));
        db.SaveChanges();
    }
}




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
