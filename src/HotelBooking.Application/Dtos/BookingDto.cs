namespace HotelBooking.Application.Dtos;

public class BookingDto
{
	public int Id { get; set; }

	public string UserId { get; set; } = default!;

	public int RoomId { get; set; }

	public DateTime CheckIn { get; set; }

	public DateTime CheckOut { get; set; }
}
