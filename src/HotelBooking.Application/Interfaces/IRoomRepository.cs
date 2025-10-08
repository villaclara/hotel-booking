using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces;

/// <summary>
/// Defines methods for managing hotel rooms in the repository.
/// </summary>
public interface IRoomRepository
{
	/// <summary>
	/// Adds a new room to the repository asynchronously.
	/// </summary>
	/// <param name="room">The <see cref="Room"/> entity to add.</param>
	/// <returns>The added <see cref="Room"/> entity.</returns>
	Task<Room> AddAsync(Room room);

	/// <summary>
	/// Retrieves all rooms associated with a specific hotel asynchronously.
	/// </summary>
	/// <param name="hotelId">The ID of the hotel.</param>
	/// <returns>A collection of <see cref="Room"/> entities for the specified hotel.</returns>
	Task<IEnumerable<Room>> GetAllByHotelAsync(int hotelId);

	/// <summary>
	/// Retrieves a room by its unique identifier asynchronously.
	/// </summary>
	/// <param name="id">The room ID.</param>
	/// <returns>The <see cref="Room"/> entity if found; otherwise, <c>null</c>.</returns>
	Task<Room?> GetByIdAsync(int id);

	/// <summary>
	/// Updates an existing room asynchronously.
	/// </summary>
	/// <param name="room">The <see cref="Room"/> entity with updated information.</param>
	/// <returns>The updated <see cref="Room"/> entity.</returns>
	Task<Room> UpdateAsync(Room room);

	/// <summary>
	/// Deletes a room by its unique identifier asynchronously.
	/// </summary>
	/// <param name="id">The room ID to delete.</param>
	/// <returns><c>true</c> if the room was successfully deleted; otherwise, <c>false</c>.</returns>
	Task<bool> DeleteAsync(int id);
}