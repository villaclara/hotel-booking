namespace HotelBooking.Domain.Entities;

public class SimpleUser
{
	public string Id { get; set; } = default!;

	public string Email { get; set; } = default!;

	public List<Booking> Bookings { get; set; } = [];
}
