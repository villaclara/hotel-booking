using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Application.Dtos.Hotel;

/// <summary>
/// Data transfer object to pass data between service and repository layers.
/// </summary>
public class HotelDto
{
	/// <summary>
	/// Gets or sets the Id of the Hotel.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the Name of the hotel.
	/// </summary>
	[Required(ErrorMessage = "Hotel Name is mandatory.")]
	public string Name { get; set; } = default!;

	/// <summary>
	/// Gets or sets the description of the hotel.
	/// </summary>
	[Required(ErrorMessage = "Hotel Description is mandatory.")]
	public string Description { get; set; } = default!;

	/// <summary>
	/// Gets or sets the Address of the hotel.
	/// </summary>
	[Required(ErrorMessage = "Hotel City Address is mandatory.")]
	public string Address { get; set; } = default!;
}
