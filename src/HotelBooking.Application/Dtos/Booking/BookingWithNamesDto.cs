using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Application.Dtos.Booking;

public class BookingWithNamesDto
{
	public int Id { get; set; }

	public string UserId { get; set; } = default!;

	public int RoomId { get; set; }

	[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
	[Required]
	public DateTime CheckIn { get; set; }

	[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
	[Required]
	public DateTime CheckOut { get; set; }

	public string RoomDescription { get; set; } = default!;

	public string HotelName { get; set; } = default!;
}
