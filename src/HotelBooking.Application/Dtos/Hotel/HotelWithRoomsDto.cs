using HotelBooking.Application.Dtos.Room;

namespace HotelBooking.Application.Dtos.Hotel;

public class HotelWithRoomsDto
{
	/// <summary>
	/// Gets or sets the Id of the Hotel.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the Name of the hotel.
	/// </summary>
	public string Name { get; set; } = default!;

	/// <summary>
	/// Gets or sets the description of the hotel.
	/// </summary>
	public string Description { get; set; } = default!;

	/// <summary>
	/// Gets or sets the Address of the hotel.
	/// </summary>
	public string Address { get; set; } = default!;

	/// <summary>
	/// Gets or sets the list of rooms of the hotel.
	/// </summary>
	public List<RoomDto> Rooms { get; set; } = [];
}
