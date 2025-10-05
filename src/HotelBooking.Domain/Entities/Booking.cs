namespace HotelBooking.Domain.Entities;

public class Booking
{
	public int Id { get; set; }

	public string UserId { get; set; } = default!;

	public int RoomId { get; set; }

	public Room Room { get; set; } = default!;

	public DateTime CheckIn { get; set; }

	public DateTime CheckOut { get; set; }
}
