using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces;

/// <summary>
/// Defines methods for managing hotels in the repository.
/// </summary>
public interface IHotelRepository
{
	/// <summary>
	/// Adds a new hotel to the repository asynchronously.
	/// </summary>
	/// <param name="hotel">The <see cref="Hotel"/> entity to add.</param>
	/// <returns>The added <see cref="Hotel"/> entity.</returns>
	Task<Hotel> AddAsync(Hotel hotel);

	/// <summary>
	/// Retrieves all hotels asynchronously.
	/// </summary>
	/// <returns>A collection of <see cref="Hotel"/> entities.</returns>
	Task<IEnumerable<Hotel>> GetAllAsync();

	/// <summary>
	/// Retrieves all hotels with their rooms that match the specified date range and city asynchronously.
	/// </summary>
	/// <param name="checkin">The check-in date filter. If <c>null</c>, all check-in dates are included.</param>
	/// <param name="checkout">The check-out date filter. If <c>null</c>, all check-out dates are included.</param>
	/// <param name="city">The city filter. If <c>null</c>, all cities are included.</param>
	/// <returns>A collection of <see cref="Hotel"/> entities with rooms matching the criteria.</returns>
	Task<IEnumerable<Hotel>> GetAllWithRoomsByDates(DateTime? checkin, DateTime? checkout, string? city);

	/// <summary>
	/// Checks if a hotel exists by its unique identifier asynchronously.
	/// </summary>
	/// <param name="id">The hotel ID.</param>
	/// <returns><c>true</c> if the hotel exists; otherwise, <c>false</c>.</returns>
	Task<bool> ExistsAsync(int id);

	/// <summary>
	/// Retrieves a hotel by its unique identifier asynchronously.
	/// </summary>
	/// <param name="id">The hotel ID.</param>
	/// <returns>The <see cref="Hotel"/> entity if found; otherwise, <c>null</c>.</returns>
	Task<Hotel?> GetByIdAsync(int id);

	/// <summary>
	/// Updates an existing hotel asynchronously.
	/// </summary>
	/// <param name="hotel">The <see cref="Hotel"/> entity with updated information.</param>
	/// <returns>The updated <see cref="Hotel"/> entity.</returns>
	Task<Hotel> UpdateAsync(Hotel hotel);

	/// <summary>
	/// Deletes a hotel by its unique identifier asynchronously.
	/// </summary>
	/// <param name="id">The hotel ID to delete.</param>
	/// <returns><c>true</c> if the hotel was successfully deleted; otherwise, <c>false</c>.</returns>
	Task<bool> DeleteAsync(int id);
}
