using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces;

public interface IBookingRepository
{
	Task<Booking> AddAsync(Booking booking);
	Task<IEnumerable<Booking>> GetAllWithParamsAsync(string? userId = null);
	Task<Booking?> FindByIdAsync(int id);
	Task<Booking> UpdateAsync(Booking booking);
	Task<bool> DeleteAsync(int id);
}
