using HotelBooking.Application.Dtos.Hotel;
using HotelBooking.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.API.Pages;

public class IndexModel : PageModel
{
	private readonly HotelService _hotelService;

	[BindProperty(SupportsGet = true)]
	public DateTime CheckIn { get; set; }

	[BindProperty(SupportsGet = true)]
	public DateTime CheckOut { get; set; }

	[BindProperty(SupportsGet = true)]
	public string City { get; set; }
	public IEnumerable<HotelWithRoomsDto> Hotels { get; set; } = [];

	public IndexModel(HotelService hotelService)
	{
		_hotelService = hotelService;
	}

	public async Task<IActionResult> OnGet(DateTime? checkin, DateTime? checkout, string? city)
	{
		if (checkout <= checkin)
		{
			ModelState.AddModelError("Booking.CheckOut", "Check-out must be after check-in.");
			return Page();
		}

		// Default values if not passed.
		CheckIn = checkin ?? DateTime.Now;
		CheckOut = checkout ?? DateTime.Now.AddDays(1);

		if (checkin.HasValue && checkout.HasValue)
		{
			Hotels = await _hotelService.GetAvailableHotelsWithRoomsForDates(checkin.Value, checkout.Value, city);
		}
		else
		{
			Hotels = await _hotelService.GetAllWithRoomsAsync();
		}

		return Page();
	}
}
