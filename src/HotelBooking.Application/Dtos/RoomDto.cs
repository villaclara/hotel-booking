using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Application.Dtos;

public class RoomDto
{
	public int Id { get; set; }

	[Range(0, int.MaxValue)]
	[Required]
	public int HotelId { get; set; }

	public string HotelName { get; set; } = default!;

	[Range(0, int.MaxValue)]
	public decimal PricePerNight { get; set; }

	[Range(0, int.MaxValue)]
	[Required]
	public int Capacity { get; set; }
}
