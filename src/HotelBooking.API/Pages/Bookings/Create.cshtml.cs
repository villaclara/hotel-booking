using HotelBooking.Application.Dtos.Booking;
using HotelBooking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.API.Pages.Bookings;

[Authorize(Roles = "User")]
public class CreateModel : PageModel
{
	private readonly UserManager<IdentityUser> _userManager;
	private readonly RoomService _roomService;
	private readonly BookingService _bookingService;

	[BindProperty(SupportsGet = true)]
	public string Email { get; set; } = string.Empty;

	[BindProperty(SupportsGet = true)]
	public DateTime CheckIn { get; set; }

	[BindProperty(SupportsGet = true)]
	public DateTime CheckOut { get; set; }

	// Room details
	[BindProperty(SupportsGet = true)]

	public int RoomId { get; set; }
	[BindProperty(SupportsGet = true)]

	public string RoomName { get; set; } = string.Empty;
	[BindProperty(SupportsGet = true)]

	public string HotelName { get; set; } = string.Empty;
	[BindProperty(SupportsGet = true)]

	public decimal Price { get; set; }
	[BindProperty(SupportsGet = true)]

	public int Capacity { get; set; }

	public CreateModel(UserManager<IdentityUser> userManager, RoomService roomService, BookingService bookingService)
	{
		_userManager = userManager;
		_roomService = roomService;
		_bookingService = bookingService;
	}

	public async Task<IActionResult> OnGet(int roomId, DateTime? checkIn, DateTime? checkOut)
	{
		CheckIn = checkIn ?? DateTime.Now;
		CheckOut = checkOut ?? DateTime.Now;

		ModelState.Remove("Email");
		var user = await _userManager.GetUserAsync(User);
		Email = user?.UserName ?? "user@test.com";

		try
		{

			var room = await _roomService.GetRoomByIdAsync(roomId);
			if (room == null)
			{
				ModelState.AddModelError("", "Room is not found.");
				return Page();
			}

			RoomId = room.Id;
			RoomName = room.Description;
			HotelName = room.HotelName;
			Price = room.PricePerNight;
			Capacity = room.Capacity;

			return Page();
		}
		catch
		{
			return RedirectToPage("/Hotels/Index");
		}
	}

	public async Task<IActionResult> OnPostAsync()
	{
		if (!ModelState.IsValid)
		{
			ModelState.AddModelError("", "Room is not found.");
			return Page();
		}

		try
		{
			var user = await _userManager.GetUserAsync(User);
			var userId = user!.Id;
			var booking = new BookingDto
			{
				CheckIn = CheckIn,
				CheckOut = CheckOut,
				RoomId = RoomId,
				UserId = userId,
			};

			var result = await _bookingService.CreateAsync(booking);
			// Save booking to database here
			return RedirectToPage("/Bookings/Index");
		}
		catch (ArgumentException)
		{
			TempData["Message"] = "Room is not available for selected dates.";
			return Page();
		}
		catch (Exception ex)
		{
			TempData["Message"] = ex.Message;
			return Page();
		}
	}
}
