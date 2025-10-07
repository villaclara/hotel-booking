using HotelBooking.Application.Dtos.Booking;
using HotelBooking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.API.Pages.Bookings;

[Authorize(Roles = "User")]
public class IndexModel : PageModel
{
	private readonly BookingService _bookingService;
	private readonly UserManager<IdentityUser> _userManager;

	public IndexModel(BookingService bookingService, UserManager<IdentityUser> userManager)
	{
		_bookingService = bookingService;
		_userManager = userManager;
	}

	public IEnumerable<BookingWithNamesDto> Bookings { get; set; }

	public async Task OnGetAsync()
	{
		var user = await _userManager.GetUserAsync(User);
		if (user != null)
		{
			Bookings = await _bookingService.GetAllWithNamesAsync(user.Id);
		}
	}
}
