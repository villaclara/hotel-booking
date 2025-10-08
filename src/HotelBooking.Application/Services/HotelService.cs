using System.Data;
using HotelBooking.Application.Dtos.Hotel;
using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Application.Services;

/// <summary>
/// Provides CRUD operations related to Hotels.
/// </summary>
public class HotelService
{
	private readonly IHotelRepository _hotelRepository;
	private readonly ILogger<HotelService> _logger;

	public HotelService(IHotelRepository hotelRepository, ILogger<HotelService> logger)
	{
		_hotelRepository = hotelRepository;
		_logger = logger;
	}

	/// <summary>
	/// Creates a new hotel asynchronously.
	/// </summary>
	/// <param name="hotelDto">The hotel data to create.</param>
	/// <returns>The created <see cref="HotelDto"/>.</returns>
	/// <exception cref="InvalidOperationException">If creation fails.</exception>
	public async Task<HotelDto> CreateAsync(HotelDto hotelDto)
	{
		ArgumentNullException.ThrowIfNullOrEmpty(hotelDto.Name);
		ArgumentNullException.ThrowIfNullOrEmpty(hotelDto.Description);
		ArgumentNullException.ThrowIfNullOrEmpty(hotelDto.Address);

		var hotel = new Hotel
		{
			Name = hotelDto.Name,
			Description = hotelDto.Description,
			Address = hotelDto.Address,
		};

		var created = await _hotelRepository.AddAsync(hotel);

		if (created == null)
		{
			_logger.LogError("{@Method} - Hotel Repository failed to create a hotel for {@hotelname}.", nameof(CreateAsync), hotelDto.Name);
			throw new InvalidOperationException("Failed to create a hotel.");
		}

		_logger.LogInformation("{@Method} - Created new hotel ({@id}, {@name}).", nameof(CreateAsync), created.Id, created.Name);

		return new HotelDto
		{
			Id = created.Id,
			Name = created.Name,
			Address = created.Address,
			Description = created.Description,
		};
	}

	/// <summary>
	/// Retrieves a hotel by its ID asynchronously.
	/// </summary>
	/// <param name="id">The hotel ID.</param>
	/// <returns>The <see cref="HotelDto"/> if found; otherwise, <c>null</c>.</returns>
	public async Task<HotelDto?> GetHotelByIdAsync(int id)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

		var hotel = await _hotelRepository.GetByIdAsync(id);

		return hotel != null
			? new HotelDto
			{
				Id = hotel.Id,
				Name = hotel.Name,
				Address = hotel.Address,
				Description = hotel.Description
			}
			: null;
	}


	/// <summary>
	/// Retrieves all hotels asynchronously.
	/// </summary>
	/// <returns>A collection of <see cref="HotelDto"/>.</returns>
	public async Task<IEnumerable<HotelDto>> GetAllAsync()
	{
		var hotels = await _hotelRepository.GetAllAsync();
		return hotels.Select(h => new HotelDto
		{
			Id = h.Id,
			Name = h.Name,
			Address = h.Address,
			Description = h.Description,
		});
	}

	/// <summary>
	/// Retrieves all hotels with their rooms asynchronously.
	/// </summary>
	/// <returns>A collection of <see cref="HotelWithRoomsDto"/>.</returns>
	public async Task<IEnumerable<HotelWithRoomsDto>> GetAllWithRoomsAsync()
	{
		var hotels = await _hotelRepository.GetAllAsync();
		return hotels.Select(h => new HotelWithRoomsDto
		{
			Id = h.Id,
			Name = h.Name,
			Address = h.Address,
			Description = h.Description,
			Rooms = [.. h.Rooms.Select(r => new Dtos.Room.RoomDto
			{
				Id = r.Id,
				Description = r.Description,
				Capacity = r.Capacity,
				HotelId = h.Id,
				HotelName = h.Name,
				PricePerNight = r.PricePerNight,
			})],
		});
	}

	/// <summary>
	/// Retrieves available hotels with rooms for a specific date range and optional city asynchronously.
	/// </summary>
	/// <param name="startDate">The start date.</param>
	/// <param name="endDate">The end date.</param>
	/// <param name="city">Optional city filter.</param>
	/// <returns>A collection of <see cref="HotelWithRoomsDto"/>.</returns>
	/// <exception cref="ArgumentException">If date range is invalid.</exception>
	public async Task<IEnumerable<HotelWithRoomsDto>> GetAvailableHotelsWithRoomsForDates(DateTime startDate, DateTime endDate, string? city)
	{
		if (startDate >= endDate)
		{
			throw new ArgumentException("Incorret dates range");
		}

		var hotels = await _hotelRepository.GetAllWithRoomsByDates(startDate, endDate, city);
		return hotels.Select(h => new HotelWithRoomsDto
		{
			Id = h.Id,
			Name = h.Name,
			Address = h.Address,
			Description = h.Description,
			Rooms = [.. h.Rooms.Select(r => new Dtos.Room.RoomDto
			{
				Id = r.Id,
				Description = r.Description,
				Capacity = r.Capacity,
				HotelId = h.Id,
				HotelName = h.Name,
				PricePerNight = r.PricePerNight,
			})],
		});
	}

	/// <summary>
	/// Updates an existing hotel asynchronously.
	/// </summary>
	/// <param name="hotelDto">The updated hotel data.</param>
	/// <returns>The updated <see cref="HotelDto"/>.</returns>
	/// <exception cref="KeyNotFoundException">If the hotel does not exist.</exception>
	/// <exception cref="InvalidOperationException">If update fails.</exception>
	public async Task<HotelDto> UpdateAsync(HotelDto hotelDto)
	{
		ArgumentNullException.ThrowIfNullOrEmpty(hotelDto.Name);
		ArgumentNullException.ThrowIfNullOrEmpty(hotelDto.Description);
		ArgumentNullException.ThrowIfNullOrEmpty(hotelDto.Address);

		var hotel = await _hotelRepository.GetByIdAsync(hotelDto.Id);
		if (hotel == null)
		{
			_logger.LogWarning("{@Method} - Hotel with id {@id} was not found in db.", nameof(UpdateAsync), hotelDto.Id);
			throw new KeyNotFoundException($"Hotel with Id {hotelDto.Id} not found.");
		}

		try
		{
			hotel.Name = hotelDto.Name;
			hotel.Description = hotelDto.Description;
			hotel.Address = hotelDto.Address;

			var result = await _hotelRepository.UpdateAsync(hotel);
			return new HotelDto
			{
				Id = result.Id,
				Name = result.Name,
				Address = result.Address,
				Description = result.Description,
			};
		}
		catch (Exception ex)
		{
			_logger.LogError("{@Method} - Hotel repository failed to update the hotel with id {@id}. Ex - {@ex}", nameof(UpdateAsync), hotelDto.Id, ex.Message);
			throw new InvalidOperationException("Failed to update the hotel.");
		}
	}

	/// <summary>
	/// Deletes a hotel by its ID asynchronously.
	/// </summary>
	/// <param name="id">The hotel ID.</param>
	/// <returns><c>true</c> if deletion succeeds; otherwise, <c>false</c>.</returns>
	/// <exception cref="InvalidOperationException">If deletion fails.</exception>
	public async Task<bool> DeleteAsync(int id)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(id, 1);

		try
		{
			var result = await _hotelRepository.DeleteAsync(id);
			_logger.LogInformation("{@Method} - Hotel with id {@id} deleted.", nameof(DeleteAsync), id);
			return result;
		}
		catch (Exception ex)
		{
			_logger.LogError("{@Method} - Hotel repository failed to delete the hotel with id {@id}. Ex - {@ex}", nameof(DeleteAsync), id, ex.Message);
			throw new InvalidOperationException("Failed to delete the hotel.");
		}
	}
}
