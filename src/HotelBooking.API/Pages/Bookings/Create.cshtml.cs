using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.API.Pages.Bookings;

public class CreateModel : PageModel
{
	[BindProperty]
	public string FullName { get; set; }

	[BindProperty]
	public string Email { get; set; }

	[BindProperty(SupportsGet = true)]
	public DateTime CheckIn { get; set; }

	[BindProperty(SupportsGet = true)]
	public DateTime CheckOut { get; set; }

	// Room details
	public string RoomName { get; set; }
	public string HotelName { get; set; }
	public decimal Price { get; set; }

	public void OnGet(int roomId, DateTime? checkIn, DateTime? checkOut)
	{
		CheckIn = checkIn ?? DateTime.Now;
		CheckOut = checkOut ?? DateTime.Now;

		// Load room data from database by roomId
		RoomName = $"Room {roomId}";
		HotelName = "Grand Hotel";
		Price = 120;
	}

	public IActionResult OnPost()
	{
		if (!ModelState.IsValid)
		{
			return Page();
		}

		// Save booking to database here
		TempData["Message"] = "Booking successfully created!";
		return RedirectToPage("/Bookings/Index");
	}
}
