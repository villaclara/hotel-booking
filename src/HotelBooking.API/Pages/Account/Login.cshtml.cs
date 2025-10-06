using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.API.Pages.Account;

public class LoginModel : PageModel
{
	public void OnGet()
	{
	}

	public class InputModel
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public bool RememberMe { get; set; }
	}

	[BindProperty]
	public InputModel Input { get; set; }
}
