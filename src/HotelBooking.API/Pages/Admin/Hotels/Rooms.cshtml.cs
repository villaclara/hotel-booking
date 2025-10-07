using HotelBooking.Application.Dtos.Room;
using HotelBooking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.API.Pages.Admin.Hotels;

[Authorize(Roles = "Admin")]
public class RoomsModel : PageModel
{
	private readonly RoomService _roomService;
	private readonly HotelService _hotelService;

	public RoomsModel(RoomService roomService, HotelService hotelService)
	{
		_roomService = roomService;
		_hotelService = hotelService;
	}

	[BindProperty(SupportsGet = true)]
	public int HotelId { get; set; }

	public string HotelName { get; set; } = string.Empty;

	public List<RoomDto> Rooms { get; set; } = new();

	public async Task OnGetAsync()
	{
		var hotel = await _hotelService.GetHotelByIdAsync(HotelId);

		HotelName = hotel.Name;

		var rooms = await _roomService.GetAllByHotelAsync(HotelId);
		Rooms = rooms.ToList();
	}

	public async Task<IActionResult> OnPostAsync(string action, RoomDto roomDto)
	{

		if (action == "Add")
		{
			await _roomService.CreateAsync(roomDto);
		}
		else if (action == "Save")
		{
			await _roomService.UpdateAsync(roomDto);
		}
		else if (action == "Delete")
		{
			await _roomService.DeleteAsync(roomDto.Id);
		}

		return RedirectToPage(new { hotelId = roomDto.HotelId });
	}
}
