using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Application.Dtos;

public class BookingDto
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
}
