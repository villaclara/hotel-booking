using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Application.Dtos.Booking;

/// <summary>
/// Data transfer object to pass data between service and repository layers.
/// </summary>
public class BookingDto
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
	/// Gets or sets the Room id of boooking.
	/// </summary>
	public int RoomId { get; set; }

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
