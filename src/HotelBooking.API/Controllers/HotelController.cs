using HotelBooking.Application.Dtos.Hotel;
using HotelBooking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class HotelController : ControllerBase
{
	private readonly HotelService _hotelService;

	public HotelController(HotelService hotelService)
	{
		_hotelService = hotelService;
	}

	[AllowAnonymous]
	[HttpGet(Name = "GetAllHotels")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAllHotels()
	{
		var hotels = await _hotelService.GetAllAsync();

		return Ok(hotels);
	}

	[AllowAnonymous]
	[HttpGet("{id:int}", Name = "GetHotelById")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetHotelById(int id)
	{
		if (id <= 0)
		{
			return BadRequest("Id must be greater or equal to 1.");
		}

		var hotel = await _hotelService.GetHotelByIdAsync(id);
		if (hotel == null)
		{
			return NotFound();
		}

		return Ok(hotel);
	}

	[AllowAnonymous]
	[HttpGet("free")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> SearchAvailableHotelsForDates([FromQuery] DateTime checkIn, [FromQuery] DateTime checkOut, [FromQuery] string? city = null)
	{
		if (checkIn == default || checkOut == default)
		{
			return BadRequest("Invalid date range.");
		}

		city = city?.Trim().ToLower();

		var hotels = await _hotelService.GetAvailableHotelsWithRoomsForDates(checkIn, checkOut, city);

		if (!hotels.Any())
		{
			return NotFound("No hotels or rooms available in the selected date range.");
		}

		return Ok(hotels);
	}

	[HttpPost(Name = "AddHotel")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> AddHotel([FromBody] HotelDto hotelDto)
	{
		if (hotelDto == null)
		{
			return BadRequest("Body is null.");
		}

		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		try
		{
			var createdHotel = await _hotelService.CreateAsync(hotelDto);
			return CreatedAtAction(nameof(GetHotelById), new { id = createdHotel.Id }, createdHotel);
		}
		catch (ArgumentNullException)
		{
			return BadRequest("Incorrect values passed to server.");
		}
		catch (Exception)
		{
			return Problem(detail: "An error occurred while processing your request.", statusCode: StatusCodes.Status500InternalServerError);
		}
	}

	[HttpPut("{id:int}", Name = "UpdateHotel")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> UpdateHotel(int id, [FromBody] HotelDto hotelDto)
	{
		hotelDto.Id = id;

		if (!ModelState.IsValid || hotelDto == null)
		{
			return BadRequest(ModelState);
		}

		try
		{
			var result = await _hotelService.UpdateAsync(hotelDto);
			return Ok(result);
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
		catch (ArgumentException)
		{
			return BadRequest("Incorrect values passed to server.");
		}
		catch (Exception)
		{
			return Problem(detail: "An error occurred while processing your request.", statusCode: StatusCodes.Status500InternalServerError);
		}
	}

	[HttpDelete("{id:int}", Name = "DeleteHotel")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> DeleteHotel(int id)
	{
		try
		{
			var result = await _hotelService.DeleteAsync(id);
			return result ? NoContent() : Problem(detail: "Delteing failed.", statusCode: StatusCodes.Status500InternalServerError);
		}
		catch (ArgumentOutOfRangeException ex)
		{
			return BadRequest(ex.Message);
		}
		catch (Exception)
		{
			return Problem(detail: "An error occurred while processing your request.", statusCode: StatusCodes.Status500InternalServerError);
		}
	}
}
