using HotelBooking.Application.Dtos.Room;
using HotelBooking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.API.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class RoomController : ControllerBase
{
	private readonly RoomService _roomService;

	public RoomController(RoomService roomService)
	{
		_roomService = roomService;
	}

	[AllowAnonymous]
	[HttpGet(Name = "GetAllRoomsInHotel")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAllRoomsForHotel([FromQuery] int hotelId)
	{
		var rooms = await _roomService.GetAllByHotelAsync(hotelId);
		return Ok(rooms);
	}

	[AllowAnonymous]
	[HttpGet("{id:int}", Name = "GetRoomById")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetRoomById(int id)
	{
		if (id <= 0)
		{
			return BadRequest("Id must be greater or equal to 1.");
		}

		var room = await _roomService.GetRoomByIdAsync(id);
		if (room == null)
		{
			return NotFound();
		}

		return Ok(room);
	}

	[HttpPost(Name = "AddRoom")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> AddRoom([FromBody] RoomDto roomDto)
	{
		if (roomDto == null)
		{
			return BadRequest("Body is null.");
		}

		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		try
		{
			var createdRoom = await _roomService.CreateAsync(roomDto);
			return CreatedAtAction(nameof(GetRoomById), new { id = createdRoom.Id }, createdRoom);
		}
		catch (ArgumentNullException)
		{
			return BadRequest("Incorrect values passed to server.");
		}
		catch (KeyNotFoundException)
		{
			return NotFound($"Hotel {roomDto.HotelId} was not found.");
		}
		catch (Exception)
		{
			return Problem(detail: "An error occurred while processing your request.", statusCode: StatusCodes.Status500InternalServerError);
		}
	}

	[HttpPut("{id:int}", Name = "UpdateRoom")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomDto roomDto)
	{
		roomDto.Id = id;

		if (!ModelState.IsValid || roomDto == null)
		{
			return BadRequest(ModelState);
		}

		try
		{
			var result = await _roomService.UpdateAsync(roomDto);
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

	[HttpDelete("{id:int}", Name = "DeleteRoom")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> DeleteRoom(int id)
	{
		try
		{
			var result = await _roomService.DeleteAsync(id);
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
