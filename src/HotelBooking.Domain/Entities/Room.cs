namespace HotelBooking.Domain.Entities;

/// <summary>
/// Represents a room within a hotel that can be booked by guests.
/// </summary>
public class Room
{
	/// <summary>
	/// Gets or sets the unique identifier for the room.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the identifier of the hotel this room belongs to.
	/// </summary>
	public int HotelId { get; set; }

	/// <summary>
	/// Gets or sets the hotel this room belongs to.
	/// </summary>
	public Hotel Hotel { get; set; } = default!;

	/// <summary>
	/// Gets or sets the price per night for staying in this room.
	/// </summary>
	public decimal PricePerNight { get; set; }

	/// <summary>
	/// Gets or sets the maximum number of guests the room can accommodate.
	/// </summary>
	public int Capacity { get; set; }

	/// <summary>
	/// Gets or sets the collection of bookings made for this room.
	/// </summary>
	public List<Booking> Bookings { get; set; } = [];
}
