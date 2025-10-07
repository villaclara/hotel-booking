using HotelBooking.Application.Dtos.Booking;
using HotelBooking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[Authorize(Roles = "User")]
[Route("api/[controller]")]
[ApiController]
public class BookingController : ControllerBase
{
	private readonly BookingService _bookingService;

	public BookingController(BookingService bookingService)
	{
		_bookingService = bookingService;
	}

	[HttpPost]
	public async Task<IActionResult> AddBooking([FromBody] BookingDto bookingDto)
	{
		if (!ModelState.IsValid || bookingDto == null)
		{
			return BadRequest(ModelState);
		}

		try
		{
			var booking = await _bookingService.CreateAsync(bookingDto);
			return CreatedAtAction(nameof(AddBooking), new { id = booking.Id }, booking);
		}
		catch (ArgumentNullException)
		{
			return BadRequest("Incorrect values passed to server.");
		}
		catch (ArgumentOutOfRangeException)
		{
			return BadRequest("Incorrect values passed to server.");
		}
		catch (KeyNotFoundException ex)
		{
			return BadRequest(ex.Message);
		}
		catch (Exception)
		{
			return Problem(detail: "An error occurred while processing your request.", statusCode: StatusCodes.Status500InternalServerError);
		}
	}

	[HttpGet("{id:int}")]
	public async Task<IActionResult> GetBooking(int id)
	{
		var result = await _bookingService.GetByIdAsync(id);
		if (result == null)
		{
			return NotFound($"Booking with id {id} not found.");
		}

		return Ok(result);
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> DeleteBooking(int id)
	{
		try
		{
			var result = await _bookingService.DeleteAsync(id);
			return result ? NoContent() : Problem(detail: "Deleting failed.", statusCode: StatusCodes.Status500InternalServerError);
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
