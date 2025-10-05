namespace HotelBooking.Application.Dtos;

public class RoomDto
{
	public int Id { get; set; }

	public int HotelId { get; set; }

	public string HotelName { get; set; } = default!;

	public decimal PricePerNight { get; set; }

	public int Capacity { get; set; }
}
