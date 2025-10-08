using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.API.Pages;

public class ErrorModel : PageModel
{
	public string Message { get; set; } = string.Empty;

	public void OnGet()
	{
		Message = "An unexpected error occurred. Please try again later.";
	}
}
