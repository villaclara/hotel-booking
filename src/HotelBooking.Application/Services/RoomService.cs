using HotelBooking.Application.Dtos.Room;
using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Application.Services;

/// <summary>
/// Provides CRUD operations related to Rooms of Hotels.
/// </summary>
public class RoomService
{
	private readonly IRoomRepository _roomRepository;
	private readonly IHotelRepository _hotelRepository;
	private readonly ILogger<RoomService> _logger;

	public RoomService(IRoomRepository roomRepository, IHotelRepository hotelRepository, ILogger<RoomService> logger)
	{
		_roomRepository = roomRepository;
		_hotelRepository = hotelRepository;
		_logger = logger;
	}

	/// <summary>
	/// Creates a new room asynchronously.
	/// </summary>
	/// <param name="roomDto">The room data to create.</param>
	/// <returns>The created <see cref="RoomDto"/>.</returns>
	/// <exception cref="KeyNotFoundException">If the hotel does not exist or the created room is missing.</exception>
	/// <exception cref="InvalidOperationException">If room creation fails.</exception>
	public async Task<RoomDto> CreateAsync(RoomDto roomDto)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(roomDto.PricePerNight, 1);
		ArgumentOutOfRangeException.ThrowIfLessThan(roomDto.Capacity, 1);
		ArgumentOutOfRangeException.ThrowIfLessThan(roomDto.HotelId, 1);
		ArgumentNullException.ThrowIfNullOrEmpty(roomDto.Description);

		var hotelExists = await _hotelRepository.ExistsAsync(roomDto.HotelId);
		if (!hotelExists)
		{
			_logger.LogWarning("{@Method} - Hotel with id {@id} not foun when creating room.", nameof(CreateAsync), roomDto.HotelId);
			throw new KeyNotFoundException($"Hotel with id {roomDto.HotelId} not found.");
		}

		var room = new Room
		{
			HotelId = roomDto.HotelId,
			Description = roomDto.Description,
			PricePerNight = roomDto.PricePerNight,
			Capacity = roomDto.Capacity
		};

		var created = await _roomRepository.AddAsync(room);
		if (created == null)
		{
			_logger.LogError("{@Method} - Failed to create room for hotel {@hotelId}.", nameof(CreateAsync), roomDto.HotelId);
			throw new InvalidOperationException("Failed to create room.");
		}

		_logger.LogInformation("{@Method} - Created room ({@id}) for hotel {@hotelId}.", nameof(CreateAsync), created.Id, created.HotelId);

		var room1 = await _roomRepository.GetByIdAsync(created.Id);
		if (room1 == null)
		{
			throw new KeyNotFoundException("{@Method} - Created room was not found in database.");
		}

		return new RoomDto
		{
			Id = room1.Id,
			Description = room1.Description,
			HotelId = room1.HotelId,
			HotelName = room1.Hotel.Name,
			PricePerNight = room1.PricePerNight,
			Capacity = room1.Capacity
		};
	}

	/// <summary>
	/// Retrieves a room by ID asynchronously.
	/// </summary>
	/// <param name="id">The room ID.</param>
	/// <returns>The <see cref="RoomDto"/> if found; otherwise, <c>null</c>.</returns>
	public async Task<RoomDto?> GetRoomByIdAsync(int id)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

		var room = await _roomRepository.GetByIdAsync(id);
		return room != null
			? new RoomDto
			{
				Id = room.Id,
				Description = room.Description,
				HotelId = room.HotelId,
				HotelName = room.Hotel.Name,
				PricePerNight = room.PricePerNight,
				Capacity = room.Capacity
			}
			: null;
	}

	/// <summary>
	/// Retrieves all rooms for a specific hotel asynchronously.
	/// </summary>
	/// <param name="hotelId">The hotel ID.</param>
	/// <returns>A collection of <see cref="RoomDto"/>.</returns>
	public async Task<IEnumerable<RoomDto>> GetAllByHotelAsync(int hotelId)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(hotelId, 1);

		var rooms = await _roomRepository.GetAllByHotelAsync(hotelId);
		return rooms.Select(r => new RoomDto
		{
			Id = r.Id,
			Description = r.Description,
			HotelId = r.HotelId,
			HotelName = r.Hotel.Name,
			PricePerNight = r.PricePerNight,
			Capacity = r.Capacity
		});
	}

	/// <summary>
	/// Updates an existing room asynchronously.
	/// </summary>
	/// <param name="roomDto">The updated room data.</param>
	/// <returns>The updated <see cref="RoomDto"/>.</returns>
	/// <exception cref="KeyNotFoundException">If the room does not exist.</exception>
	/// <exception cref="InvalidOperationException">If update fails.</exception>
	public async Task<RoomDto> UpdateAsync(RoomDto roomDto)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(roomDto.Id, 1);
		ArgumentOutOfRangeException.ThrowIfLessThan(roomDto.PricePerNight, 1);
		ArgumentOutOfRangeException.ThrowIfLessThan(roomDto.Capacity, 1);
		ArgumentNullException.ThrowIfNullOrEmpty(roomDto.Description);

		var existing = await _roomRepository.GetByIdAsync(roomDto.Id);
		if (existing == null)
		{
			_logger.LogWarning("{@Method} - Room with id {@id} not found.", nameof(UpdateAsync), roomDto.Id);
			throw new KeyNotFoundException($"Room with Id {roomDto.Id} not found.");
		}

		try
		{
			existing.PricePerNight = roomDto.PricePerNight;
			existing.Capacity = roomDto.Capacity;
			existing.Description = roomDto.Description;

			var updated = await _roomRepository.UpdateAsync(existing);

			return new RoomDto
			{
				Id = updated.Id,
				Description = updated.Description,
				HotelId = updated.HotelId,
				HotelName = updated.Hotel.Name,
				PricePerNight = updated.PricePerNight,
				Capacity = updated.Capacity
			};
		}
		catch (Exception ex)
		{
			_logger.LogError("{@Method} - Failed to update room {@id}. Ex: {@ex}", nameof(UpdateAsync), roomDto.Id, ex.Message);
			throw new InvalidOperationException("Failed to update room.");
		}
	}

	/// <summary>
	/// Deletes a room by ID asynchronously.
	/// </summary>
	/// <param name="roomId">The room ID.</param>
	/// <returns><c>true</c> if deletion succeeds; otherwise, <c>false</c>.</returns>
	/// <exception cref="InvalidOperationException">If deletion fails.</exception>
	public async Task<bool> DeleteAsync(int roomId)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(roomId, 1);

		try
		{
			var result = await _roomRepository.DeleteAsync(roomId);
			_logger.LogInformation("{@Method} - Deleted room with id {@id}.", nameof(DeleteAsync), roomId);
			return result;
		}
		catch (Exception ex)
		{
			_logger.LogError("{@Method} - Failed to delete room {@id}. Ex: {@ex}", nameof(DeleteAsync), roomId, ex.Message);
			throw new InvalidOperationException("Failed to delete room.");
		}
	}
}
