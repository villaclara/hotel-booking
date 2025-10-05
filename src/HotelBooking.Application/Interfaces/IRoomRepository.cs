using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.Interfaces;

public interface IRoomRepository
{
	Task<Room> AddAsync(Room room);
	Task<IEnumerable<Room>> GetAllAsyncByHotel(int hotelId);
	Task<Room?> FindByIdAsync(int id);
	Task<Room> UpdateAsync(Room room);
	Task<bool> DeleteAsync(int id);
}
