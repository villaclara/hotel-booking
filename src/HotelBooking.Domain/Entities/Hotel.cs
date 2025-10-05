namespace HotelBooking.Domain.Entities;

/// <summary>
/// Represents a hotel entity in the booking system.
/// </summary>
public class Hotel
{
	/// <summary>
	/// Gets or sets the unique identifier for the hotel.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the name of the hotel.
	/// </summary>
	public string Name { get; set; } = default!;

	/// <summary>
	/// Gets or sets the physical address of the hotel.
	/// </summary>
	public string Address { get; set; } = default!;

	/// <summary>
	/// Gets or sets the detailed description of the hotel.
	/// </summary>
	public string Description { get; set; } = default!;

	/// <summary>
	/// Gets or sets the collection of rooms available in the hotel.
	/// </summary>
	public List<Room> Rooms { get; set; } = [];
}
