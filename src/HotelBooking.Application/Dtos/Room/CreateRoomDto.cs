using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Application.Dtos.Room;

/// <summary>
/// Data transfer object to pass data between service and repository layers.
/// </summary>
public class CreateRoomDto
{
	/// <summary>
	/// Gets or sets the hotel Id of room.
	/// </summary>
	[Range(0, int.MaxValue)]
	[Required]
	public int HotelId { get; set; }

	/// <summary>
	/// Gets or sets Description of room.
	/// </summary>
	[Required(ErrorMessage = "Room Description is mandatory.")]
	public string Description { get; set; } = default!;

	/// <summary>
	/// Gets or sets the Price for room.
	/// </summary>
	[Range(0, int.MaxValue)]
	public decimal PricePerNight { get; set; }

	/// <summary>
	/// Gets or sets the Capacity of room.
	/// </summary>
	[Range(0, int.MaxValue)]
	[Required]
	public int Capacity { get; set; }
}
