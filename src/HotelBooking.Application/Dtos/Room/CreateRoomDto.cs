using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Application.Dtos.Room;

public class CreateRoomDto
{
	[Range(0, int.MaxValue)]
	[Required]
	public int HotelId { get; set; }

	[Range(0, int.MaxValue)]
	public decimal PricePerNight { get; set; }

	[Range(0, int.MaxValue)]
	[Required]
	public int Capacity { get; set; }
}
