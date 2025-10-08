using HotelBooking.Application.Dtos.Booking;
using HotelBooking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.API.Pages.Bookings;

[Authorize(Roles = "User")]
public class IndexModel : PageModel
{
	private readonly BookingService _bookingService;
	private readonly UserManager<IdentityUser> _userManager;

	[BindProperty]
	public string FeedbackMessage { get; set; }
	public IEnumerable<BookingWithNamesDto> Bookings { get; set; }

	public IndexModel(BookingService bookingService, UserManager<IdentityUser> userManager)
	{
		_bookingService = bookingService;
		_userManager = userManager;
	}

	public async Task OnGetAsync()
	{
		var user = await _userManager.GetUserAsync(User);
		if (user != null)
		{
			Bookings = await _bookingService.GetAllWithNamesAsync(user.Id);
		}
	}

	public async Task<IActionResult> OnPostCancelAsync(int id)
	{
		bool isDeleted = false;
		try
		{
			isDeleted = await _bookingService.DeleteAsync(id);

			if (!isDeleted)
			{
				FeedbackMessage = "Unable to cancel booking. Please try again.";
				return Page();
			}

			FeedbackMessage = string.Empty;
			return RedirectToPage();
		}
		catch
		{
			FeedbackMessage = "Internal Error when cancelling booking.";
			return Page();
		}
	}
}
