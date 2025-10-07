using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Application.Dtos.Room;

public class RoomDto
{
	public int Id { get; set; }

	[Range(0, int.MaxValue)]
	[Required]
	public int HotelId { get; set; }

	[Required(ErrorMessage = "Room Description is mandatory.")]
	public string Description { get; set; } = default!;

	public string HotelName { get; set; } = default!;

	[Range(0, int.MaxValue)]
	public decimal PricePerNight { get; set; }

	[Range(0, int.MaxValue)]
	[Required]
	public int Capacity { get; set; }
}
