namespace MeetingRoomBooking.Domain;

public class Room
{
    public Guid Id { get; }
    public string Name { get; }
    public int Capacity { get; }
    public bool IsActive { get; }

    public Room(Guid id, string name, int capacity, bool isActive = true)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Room name cannot be empty.");

        if (capacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero.");

        Id = id;
        Name = name;
        Capacity = capacity;
        IsActive = isActive;
    }
}
