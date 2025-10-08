using HotelBooking.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatsController : ControllerBase
{
	private readonly StatsService _statisticsService;

	public StatsController(StatsService statisticsService)
	{
		_statisticsService = statisticsService;
	}

	[HttpGet("bookings-per-hotel")]
	public async Task<IActionResult> GetBookingsPerHotel(DateTime? from = null, DateTime? to = null)
	{
		var stats = await _statisticsService.GetBookingsStatsAsync(from, to);
		return Ok(stats);
	}
}
