namespace HotelBooking.Application.Dtos;

public class HotelWithRoomsDto
{
	public int Id { get; set; }

	public string Name { get; set; } = default!;

	public string Address { get; set; } = default!;

	public string Description { get; set; } = default!;

	public List<RoomDto> Rooms { get; set; } = [];
}
