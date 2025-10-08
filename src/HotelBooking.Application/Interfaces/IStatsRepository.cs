using HotelBooking.Application.Dtos.Booking;

namespace HotelBooking.Application.Interfaces;

/// <summary>
/// Defines methods for retrieving statistical data related to hotels and bookings.
/// </summary>
public interface IStatsRepository
{
	/// <summary>
	/// Retrieves the number of bookings per hotel within an optional date range asynchronously.
	/// </summary>
	/// <param name="from">The start date to filter bookings. If <c>null</c>, no start date filter is applied.</param>
	/// <param name="to">The end date to filter bookings. If <c>null</c>, no end date filter is applied.</param>
	/// <returns>
	/// A collection of <see cref="HotelBookingStatsDto"/> containing hotel information
	/// and the corresponding booking count.
	/// </returns>
	Task<IEnumerable<HotelBookingStatsDto>> GetBookingsCountPerHotelAsync(DateTime? from = null, DateTime? to = null);
}
