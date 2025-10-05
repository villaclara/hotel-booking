namespace HotelBooking.Domain.Entities;

public class Hotel
{
	public int Id { get; set; }

	public string Name { get; set; } = default!;

	public string Address { get; set; } = default!;

	public string Description { get; set; } = default!;

	public List<Room> Rooms { get; set; } = [];
}
