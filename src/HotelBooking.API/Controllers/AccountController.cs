using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
	private readonly UserManager<IdentityUser> _userManager;
	private readonly SignInManager<IdentityUser> _signInManager;

	public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
	{
		_userManager = userManager;
		_signInManager = signInManager;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterDto model)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		var user = new IdentityUser { UserName = model.Email, Email = model.Email };
		var result = await _userManager.CreateAsync(user, model.Password);

		if (!result.Succeeded)
		{
			return BadRequest(result.Errors);
		}

		// Optional: automatically sign in the new user
		await _signInManager.SignInAsync(user, isPersistent: false);

		return Ok(new { message = "User registered and logged in successfully" });
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginDto model)
	{
		var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

		if (result.Succeeded)
		{
			return Ok(new { message = "Login successful" });
		}

		return Unauthorized(new { message = "Invalid login attempt" });
	}

	[Authorize]
	[HttpPost("logout")]
	public async Task<IActionResult> Logout()
	{
		await _signInManager.SignOutAsync();
		return Ok(new { message = "Logout successful" });
	}

	[Authorize]
	[HttpGet("user")]
	public IActionResult GetCurrentUser()
	{
		return Ok(new
		{
			userName = User.Identity?.Name,
			isAuthenticated = User.Identity?.IsAuthenticated ?? false
		});
	}
}

public class RegisterDto
{
	public string Email { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
}

public class LoginDto
{
	public string Email { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public bool RememberMe { get; set; }
}
