using HotelBooking.Application.Dtos.Hotel;
using HotelBooking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.API.Pages.Admin.Hotels;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
	private readonly HotelService _hotelService;

	[BindProperty]
	public List<HotelWithRoomsDto> Hotels { get; set; }

	[BindProperty]
	public int HotelId { get; set; }

	[BindProperty]
	public string Name { get; set; } = string.Empty;

	[BindProperty]
	public string Address { get; set; } = string.Empty;

	[BindProperty]
	public string Description { get; set; } = string.Empty;

	[BindProperty]
	public string Action { get; set; } = string.Empty;

	public IndexModel(HotelService hotelService)
	{
		_hotelService = hotelService;
	}

	public async Task OnGet()
	{
		var hotels = await _hotelService.GetAllWithRoomsAsync();
		Hotels = hotels.ToList();
	}

	public async Task<IActionResult> OnPostAsync()
	{
		if (Action == "Delete")
		{
			await _hotelService.DeleteAsync(HotelId);
		}

		else if (Action == "Save")
		{
			var hotel = new HotelDto
			{
				Id = HotelId,
				Name = Name,
				Address = Address,
				Description = Description,
			};

			await _hotelService.UpdateAsync(hotel);
		}

		else if (Action == "Add")
		{
			var hotel = new HotelDto
			{
				Id = HotelId,
				Name = Name,
				Address = Address,
				Description = Description,
			};

			await _hotelService.CreateAsync(hotel);
		}

		return RedirectToPage();
	}
}
