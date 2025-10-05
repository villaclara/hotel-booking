namespace HotelBooking.Application.Dtos;

public class HotelDto
{
	public int Id { get; set; }

	public string Name { get; set; } = default!;

	public string Address { get; set; } = default!;

	public string Description { get; set; } = default!;
}
