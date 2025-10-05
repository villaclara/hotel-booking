namespace HotelBooking.Domain.Entities;

/// <summary>
/// Represents a booking reservation for a room in a hotel.
/// </summary>
public class Booking
{
	/// <summary>
	/// Gets or sets the unique identifier for the booking.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the identifier of the user who made the booking.
	/// </summary>
	public string UserId { get; set; } = default!;

	/// <summary>
	/// Gets or sets the identifier of the room being booked.
	/// </summary>
	public int RoomId { get; set; }

	/// <summary>
	/// Gets or sets the room associated with this booking.
	/// </summary>
	public Room Room { get; set; } = default!;

	/// <summary>
	/// Gets or sets the check-in date and time for the booking.
	/// </summary>
	public DateTime CheckIn { get; set; }

	/// <summary>
	/// Gets or sets the check-out date and time for the booking.
	/// </summary>
	public DateTime CheckOut { get; set; }
}
