using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces;

/// <summary>
/// Defines methods for managing hotel bookings in the repository.
/// </summary>
public interface IBookingRepository
{
	/// <summary>
	/// Adds a new booking to the repository asynchronously.
	/// </summary>
	/// <param name="booking">The booking entity to add.</param>
	/// <returns>The added <see cref="Booking"/> entity.</returns>
	Task<Booking> AddAsync(Booking booking);

	/// <summary>
	/// Retrieves all bookings optionally filtered by user ID asynchronously.
	/// </summary>
	/// <param name="userId">The user ID to filter bookings. If <c>null</c>, returns all bookings.</param>
	/// <returns>A collection of <see cref="Booking"/> entities.</returns>
	Task<IEnumerable<Booking>> GetAllWithParamsAsync(string? userId = null);

	/// <summary>
	/// Retrieves a booking by its unique identifier asynchronously.
	/// </summary>
	/// <param name="id">The booking ID.</param>
	/// <returns>The <see cref="Booking"/> entity if found; otherwise, <c>null</c>.</returns>
	Task<Booking?> GetByIdAsync(int id);

	/// <summary>
	/// Deletes a booking by its unique identifier asynchronously.
	/// </summary>
	/// <param name="id">The booking ID to delete.</param>
	/// <returns><c>true</c> if the booking was successfully deleted; otherwise, <c>false</c>.</returns>
	Task<bool> DeleteAsync(int id);

	/// <summary>
	/// Checks if a room is available for the specified date range asynchronously.
	/// </summary>
	/// <param name="id">The room ID.</param>
	/// <param name="checkin">The check-in date.</param>
	/// <param name="checkout">The check-out date.</param>
	/// <returns><c>true</c> if the room is available; otherwise, <c>false</c>.</returns>
	Task<bool> IsRoomAvailableAsync(int id, DateTime checkin, DateTime checkout);
}
