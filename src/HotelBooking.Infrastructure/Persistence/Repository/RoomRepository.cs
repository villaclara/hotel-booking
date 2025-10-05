using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Persistence.Repository;

public class RoomRepository : IRoomRepository
{
	private readonly AppDbContext _context;

	public RoomRepository(AppDbContext context)
	{
		_context = context;
	}

	public async Task<Room> AddAsync(Room room)
	{
		await _context.Rooms.AddAsync(room);
		await _context.SaveChangesAsync();
		return room;
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var room = await _context.Rooms.FindAsync(id);
		if (room == null)
		{
			return false;
		}
		_context.Rooms.Remove(room);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<Room?> FindByIdAsync(int id) =>
		await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);

	public async Task<IEnumerable<Room>> GetAllAsyncByHotel(int hotelId) =>
		await _context.Rooms.Where(r => r.HotelId == hotelId).ToListAsync();

	public async Task<Room> UpdateAsync(Room room)
	{
		_context.Rooms.Update(room);
		await _context.SaveChangesAsync();
		return room;
	}
}
