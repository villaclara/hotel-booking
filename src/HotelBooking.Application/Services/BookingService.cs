using HotelBooking.Application.Dtos;
using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Application.Services;

public class BookingService
{
	private readonly IBookingRepository _bookingRepository;
	private readonly IRoomRepository _roomRepository;
	private readonly ILogger<BookingService> _logger;

	public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository, ILogger<BookingService> logger)
	{
		_bookingRepository = bookingRepository;
		_roomRepository = roomRepository;
		_logger = logger;
	}

	/// <summary>
	/// Creates a booking if room exists and dates are available.
	/// </summary>
	public async Task<BookingDto> CreateAsync(BookingDto bookingDto)
	{
		ArgumentNullException.ThrowIfNull(bookingDto);
		ArgumentOutOfRangeException.ThrowIfLessThan(bookingDto.RoomId, 1);
		ArgumentNullException.ThrowIfNullOrEmpty(bookingDto.UserId);

		if (bookingDto.CheckIn >= bookingDto.CheckOut)
		{
			throw new ArgumentException("Check-out date must be after check-in date.");
		}

		var room = await _roomRepository.GetByIdAsync(bookingDto.RoomId);
		if (room == null)
		{
			_logger.LogWarning("{@Method} - Room with id {@roomId} not found.", nameof(CreateAsync), bookingDto.RoomId);
			throw new KeyNotFoundException($"Room with id {bookingDto.RoomId} not found.");
		}

		var isAvailable = await _bookingRepository.IsRoomAvailableAsync(
			bookingDto.RoomId,
			bookingDto.CheckIn,
			bookingDto.CheckOut
		);

		if (!isAvailable)
		{
			_logger.LogWarning("{@Method} - Room {@roomId} not available for {checkIn} - {checkOut}.",
				nameof(CreateAsync), bookingDto.RoomId, bookingDto.CheckIn, bookingDto.CheckOut);
			throw new ArgumentOutOfRangeException("Room is not available for selected dates.");
		}

		var booking = new Booking
		{
			UserId = bookingDto.UserId,
			RoomId = bookingDto.RoomId,
			CheckIn = bookingDto.CheckIn,
			CheckOut = bookingDto.CheckOut
		};

		var created = await _bookingRepository.AddAsync(booking);
		if (created == null)
		{
			_logger.LogError("{@Method} - Failed to create booking for room {@roomId}.", nameof(CreateAsync), bookingDto.RoomId);
			throw new InvalidOperationException("Failed to create booking.");
		}

		_logger.LogInformation("{@Method} - Created booking {@id} for room {@roomId}.", nameof(CreateAsync), created.Id, created.RoomId);

		return new BookingDto
		{
			Id = created.Id,
			RoomId = created.RoomId,
			UserId = created.UserId,
			CheckIn = created.CheckIn,
			CheckOut = created.CheckOut
		};
	}

	public async Task<IEnumerable<BookingDto>> GetAllAsync(string? userId = null)
	{
		var bookings = await _bookingRepository.GetAllWithParamsAsync(userId);

		return bookings.Select(b => new BookingDto
		{
			Id = b.Id,
			RoomId = b.RoomId,
			UserId = b.UserId,
			CheckIn = b.CheckIn,
			CheckOut = b.CheckOut
		});
	}

	public async Task<BookingDto?> GetByIdAsync(int bookingId)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(bookingId, 1);

		var booking = await _bookingRepository.GetByIdAsync(bookingId);
		if (booking == null)
		{
			_logger.LogWarning("{@Method} - Booking with id {@id} not found.", nameof(GetByIdAsync), bookingId);
			return null;
		}

		return new BookingDto
		{
			Id = booking.Id,
			RoomId = booking.RoomId,
			UserId = booking.UserId,
			CheckIn = booking.CheckIn,
			CheckOut = booking.CheckOut
		};
	}

	public async Task<bool> DeleteAsync(int bookingId)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(bookingId, 1);

		try
		{
			var result = await _bookingRepository.DeleteAsync(bookingId);
			_logger.LogInformation("{@Method} - Booking with id {@id} deleted.", nameof(DeleteAsync), bookingId);
			return result;
		}
		catch (Exception ex)
		{
			_logger.LogError("{@Method} - Failed to delete booking {@id}. Ex: {@ex}", nameof(DeleteAsync), bookingId, ex.Message);
			throw new InvalidOperationException("Failed to delete booking.");
		}
	}
}
