using HotelBooking.Application.Dtos.Booking;
using HotelBooking.Application.Interfaces;

namespace HotelBooking.Application.Services;

/// <summary>
/// Provides operations to get the statistic.
/// </summary>
public class StatsService
{
	private readonly IStatsRepository _statsRepo;

	public StatsService(IStatsRepository statsRepo)
	{
		_statsRepo = statsRepo;
	}

	/// <summary>
	/// Retrieves the number of bookings per hotel within an optional date range asynchronously.
	/// </summary>
	/// <param name="from">The start date to filter bookings. If <c>null</c>, no start date filter is applied.</param>
	/// <param name="to">The end date to filter bookings. If <c>null</c>, no end date filter is applied.</param>
	/// <returns>
	/// A collection of <see cref="HotelBookingStatsDto"/> containing hotel information
	/// and the corresponding booking count.
	/// </returns>
	public Task<IEnumerable<HotelBookingStatsDto>> GetBookingsStatsAsync(DateTime? from = null, DateTime? to = null)
	{
		return _statsRepo.GetBookingsCountPerHotelAsync(from, to);
	}
}
