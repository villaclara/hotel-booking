namespace HotelBooking.Application.Dtos.Booking;

/// <summary>
/// Data transfer object to pass data between service and repository layers.
/// </summary>
public class HotelBookingStatsDto
{
	/// <summary>
	/// Gets or sets the Hotel Id for stats.
	/// </summary>
	public int HotelId { get; set; }

	/// <summary>
	/// Gets or sets the Hotel Name for stats.
	/// </summary>
	public string HotelName { get; set; } = default!;

	/// <summary>
	/// Gets or sets the count of bookings for hotel.
	/// </summary>
	public int BookingCount { get; set; }
}
