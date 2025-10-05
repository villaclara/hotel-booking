using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces;

public interface IRoomRepository
{
	Task<Room> AddAsync(Room room);
	Task<IEnumerable<Room>> GetAllByHotelAsync(int hotelId);
	Task<Room?> GetByIdAsync(int id);
	Task<Room> UpdateAsync(Room room);
	Task<bool> DeleteAsync(int id);
}
