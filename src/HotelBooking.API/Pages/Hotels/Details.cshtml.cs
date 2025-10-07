using HotelBooking.Application.Dtos.Hotel;
using HotelBooking.Application.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.API.Pages.Hotels;

public class DetailsModel : PageModel
{
	private readonly HotelService _hotelService;

	public DetailsModel(HotelService hotelService)
	{
		_hotelService = hotelService;
	}

	public HotelWithRoomsDto? Hotel { get; set; }

	public async Task OnGet(int id)
	{
		Hotel = await _hotelService.GetHotelWithRoomsById(id);
	}
}
