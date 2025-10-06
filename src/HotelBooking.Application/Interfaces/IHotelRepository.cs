using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces;

public interface IHotelRepository
{
	Task<Hotel> AddAsync(Hotel hotel);

	Task<IEnumerable<Hotel>> GetAllAsync();

	Task<IEnumerable<Hotel>> GetAllWithRoomsByDates(DateTime? checkin, DateTime? checkout, string? city);

	Task<bool> ExistsAsync(int id);

	Task<Hotel?> GetByIdAsync(int id);

	Task<Hotel> UpdateAsync(Hotel hotel);

	Task<bool> DeleteAsync(int id);
}
