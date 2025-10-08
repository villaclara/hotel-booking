using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Application.Dtos.Booking;

/// <summary>
/// Data transfer object to pass data between service and repository layers.
/// </summary>
public class BookingWithNamesDto
{
	/// <summary>
	/// Gets or sets the Id of booking.
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the UserId of booking.
	/// </summary>
	public string UserId { get; set; } = default!;

	/// <summary>
	/// Gets or sets the RoomId of booking.
	/// </summary>
	public int RoomId { get; set; }

	/// <summary>
	/// Gets or sets the room description for booking.
	/// </summary>
	public string RoomDescription { get; set; } = default!;

	/// <summary>
	/// Gets or sets the hotel name for booking.
	/// </summary>
	public string HotelName { get; set; } = default!;

	/// <summary>
	/// Gets or sets the arrival date.
	/// </summary>
	[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
	[Required]
	public DateTime CheckIn { get; set; }

	/// <summary>
	/// Gets or sets the leave date.
	/// </summary>
	[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
	[Required]
	public DateTime CheckOut { get; set; }
}
