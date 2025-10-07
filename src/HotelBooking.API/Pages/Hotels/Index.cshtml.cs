using HotelBooking.Application.Dtos.Hotel;
using HotelBooking.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.API.Pages.Hotels;

public class IndexModel : PageModel
{
	private readonly HotelService _hotelService;

	public IndexModel(HotelService hotelService)
	{
		_hotelService = hotelService;
	}

	//public IEnumerable<HotelDto> Hotels { get; set; } = [];
	public IEnumerable<HotelWithRoomsDto> Hotels { get; set; } = [];

	[BindProperty(SupportsGet = true)]
	public DateTime CheckIn { get; set; }

	[BindProperty(SupportsGet = true)]
	public DateTime CheckOut { get; set; }

	[BindProperty(SupportsGet = true)]
	public string City { get; set; }

	public async Task OnGet(DateTime? checkin, DateTime? checkout, string? city)
	{
		CheckIn = checkin ?? DateTime.Now;
		CheckOut = checkout ?? DateTime.Now.AddDays(1);
		//if (checkin.HasValue && checkout.HasValue)
		//{
		//	Hotels = await _hotelService.GetAvailableHotelsForDates(checkin.Value, checkout.Value, city);
		//}
		//else
		//{
		//	Hotels = await _hotelService.GetAllAsync();
		//}

		if (checkin.HasValue && checkout.HasValue)
		{
			Hotels = await _hotelService.GetAvailableHotelsWithRoomsForDates(checkin.Value, checkout.Value, city);
		}
		else
		{
			Hotels = await _hotelService.GetAllWithRoomsAsync();
		}
	}
}
