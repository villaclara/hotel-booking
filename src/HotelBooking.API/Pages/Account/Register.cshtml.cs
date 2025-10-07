using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.API.Pages.Account;

public class RegisterModel : PageModel
{
	private readonly SignInManager<IdentityUser> signInManager;
	private readonly UserManager<IdentityUser> userManager;
	private readonly IUserStore<IdentityUser> userStore;
	private readonly IUserEmailStore<IdentityUser> emailStore;
	private readonly ILogger<RegisterModel> logger;

	public RegisterModel(
		UserManager<IdentityUser> userManager,
		IUserStore<IdentityUser> userStore,
		SignInManager<IdentityUser> signInManager,
		ILogger<RegisterModel> logger)
	{
		this.userManager = userManager;
		this.userStore = userStore;
		emailStore = GetEmailStore();
		this.signInManager = signInManager;
		this.logger = logger;
	}

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[BindProperty]
	public InputModel Input { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public string ReturnUrl { get; set; }

	public void OnGet(string returnUrl = null)
	{
		ReturnUrl = returnUrl;
	}

	public async Task<IActionResult> OnPostAsync(string returnUrl = null)
	{
		returnUrl ??= Url.Content("~/");
		if (ModelState.IsValid)
		{
			var user = CreateUser();

			await userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
			await emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
			var result = await userManager.CreateAsync(user, Input.Password);

			if (result.Succeeded)
			{
				if (!await userManager.IsInRoleAsync(user, "User"))
				{
					await userManager.AddToRoleAsync(user, "User");
				}

				await signInManager.SignInAsync(user, isPersistent: false);
				return LocalRedirect(returnUrl);
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}

		// If we got this far, something failed, redisplay form
		return Page();
	}

	private IdentityUser CreateUser()
	{
		try
		{
			return Activator.CreateInstance<IdentityUser>();
		}
		catch
		{
			logger.LogError("Error when creating new ApplicationUser.");
			throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
				$"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
				$"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
		}
	}

	private IUserEmailStore<IdentityUser> GetEmailStore()
	{
		if (!userManager.SupportsUserEmail)
		{
			throw new NotSupportedException("The default UI requires a user store with email support.");
		}

		return (IUserEmailStore<IdentityUser>)userStore;
	}
}

public class InputModel
{
	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[Required]
	[EmailAddress]
	[Display(Name = "Email")]
	public string Email { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[Required]
	[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
	[DataType(DataType.Password)]
	[Display(Name = "Password")]
	public string Password { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[DataType(DataType.Password)]
	[Display(Name = "Confirm password")]
	[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
	public string ConfirmPassword { get; set; }
}
