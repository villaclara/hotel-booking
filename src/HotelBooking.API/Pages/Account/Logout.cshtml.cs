using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.API.Pages.Account;

public class LogoutModel : PageModel
{
	private readonly SignInManager<IdentityUser> _signInManager;

	public LogoutModel(SignInManager<IdentityUser> signInManager)
	{
		_signInManager = signInManager;
	}

	public async Task<IActionResult> OnPost()
	{
		await _signInManager.SignOutAsync();
		return Redirect("/Index");
	}
}
