namespace HotelBooking.Domain.Entities;

public class Room
{
	public int Id { get; set; }

	public Guid HotelId { get; set; }

	public Hotel Hotel { get; set; } = default!;

	public decimal PricePerNight { get; set; }

	public int Capacity { get; set; }

	public List<Booking> Bookings { get; set; } = [];
}
