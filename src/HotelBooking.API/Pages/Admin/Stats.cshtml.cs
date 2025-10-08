using HotelBooking.Application.Dtos.Booking;
using HotelBooking.Application.Dtos.Hotel;
using HotelBooking.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.API.Pages.Admin;

public class StatsModel : PageModel
{
	private readonly StatsService _statisticsService;
	private readonly HotelService _hotelService;

	public StatsModel(StatsService statisticsService, HotelService hotelService)
	{
		_statisticsService = statisticsService;
		_hotelService = hotelService;
	}

	// Dropdown list of all hotels
	public IList<HotelDto> Hotels { get; set; } = new List<HotelDto>();

	// Selected hotel and date range
	[BindProperty]
	public int? SelectedHotelId { get; set; }

	[BindProperty]
	public DateTime? FromDate { get; set; }

	[BindProperty]
	public DateTime? ToDate { get; set; }

	// Results
	public IList<HotelBookingStatsDto> Stats { get; set; } = new List<HotelBookingStatsDto>();

	public async Task OnGetAsync()
	{
		Hotels = (await _hotelService.GetAllAsync()).ToList();
	}

	public async Task OnPostAsync()
	{
		Hotels = (await _hotelService.GetAllAsync()).ToList();

		var from = FromDate ?? DateTime.Today.AddDays(-30);
		var to = ToDate ?? DateTime.Today;

		var stats = await _statisticsService.GetBookingsStatsAsync(from, to);

		if (SelectedHotelId.HasValue)
		{
			Stats = stats.Where(s => s.HotelId == SelectedHotelId.Value).ToList();
		}
		else
		{
			Stats = stats.ToList();
		}
	}
}
