using HotelBooking.Application.Dtos.Hotel;
using HotelBooking.Application.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.API.Pages;

public class IndexModel : PageModel
{
	private readonly HotelService _hotelService;

	public IndexModel(HotelService hotelService)
	{
		_hotelService = hotelService;
	}

	public IEnumerable<HotelDto> Hotels { get; set; } = new List<HotelDto>(capacity: 5);

	public async Task OnGet()
	{
		var hotels = await _hotelService.GetAllAsync();
		// TODO - implement hotels shuffling to display different hotels at index
		Hotels = hotels;
	}
}
