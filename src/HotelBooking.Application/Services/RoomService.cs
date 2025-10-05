using HotelBooking.Application.Dtos;
using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Application.Services;

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

	public async Task<RoomDto> CreateAsync(RoomDto roomDto)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(roomDto.PricePerNight, 1);
		ArgumentOutOfRangeException.ThrowIfLessThan(roomDto.Capacity, 1);
		ArgumentOutOfRangeException.ThrowIfLessThan(roomDto.HotelId, 1);

		var hotelExists = await _hotelRepository.ExistsAsync(roomDto.HotelId);
		if (!hotelExists)
		{
			_logger.LogWarning("{@Method} - Hotel with id {@id} not foun when creating room.", nameof(CreateAsync), roomDto.HotelId);
			throw new KeyNotFoundException($"Hotel with id {roomDto.HotelId} not found.");
		}

		var room = new Room
		{
			HotelId = roomDto.HotelId,
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

		// TODO - not sure if it will work because i do not pass the HotelName before.
		return new RoomDto
		{
			Id = created.Id,
			HotelId = created.HotelId,
			HotelName = created.Hotel.Name,
			PricePerNight = created.PricePerNight,
			Capacity = created.Capacity
		};
	}

	public async Task<RoomDto?> GetRoomByIdAsync(int id)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

		var room = await _roomRepository.GetByIdAsync(id);
		return room != null
			? new RoomDto
			{
				Id = room.Id,
				HotelId = room.HotelId,
				HotelName = room.Hotel.Name,
				PricePerNight = room.PricePerNight,
				Capacity = room.Capacity
			}
			: null;
	}

	public async Task<IEnumerable<RoomDto>> GetAllByHotelAsync(int hotelId)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(hotelId, 1);

		var rooms = await _roomRepository.GetAllByHotelAsync(hotelId);
		return rooms.Select(r => new RoomDto
		{
			Id = r.Id,
			HotelId = r.HotelId,
			HotelName = r.Hotel.Name,
			PricePerNight = r.PricePerNight,
			Capacity = r.Capacity
		});
	}

	public async Task<RoomDto> UpdateAsync(RoomDto roomDto)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(roomDto.Id, 1);
		ArgumentOutOfRangeException.ThrowIfLessThan(roomDto.PricePerNight, 1);
		ArgumentOutOfRangeException.ThrowIfLessThan(roomDto.Capacity, 1);

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

			var updated = await _roomRepository.UpdateAsync(existing);

			return new RoomDto
			{
				Id = updated.Id,
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
