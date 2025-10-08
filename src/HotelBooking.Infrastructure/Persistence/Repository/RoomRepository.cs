using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Persistence.Repository;

/// <inheritdoc cref="IRoomRepository"/>
public class RoomRepository : IRoomRepository
{
	private readonly AppDbContext _context;

	public RoomRepository(AppDbContext context)
	{
		_context = context;
	}

	/// <inheritdoc/>
	public async Task<Room> AddAsync(Room room)
	{
		var added = await _context.Rooms.AddAsync(room);
		await _context.SaveChangesAsync();
		return room;
	}

	/// <inheritdoc/>
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

	/// <inheritdoc/>
	public async Task<Room?> GetByIdAsync(int id) =>
		await _context.Rooms.Include(r => r.Hotel).FirstOrDefaultAsync(r => r.Id == id);

	/// <inheritdoc/>
	public async Task<IEnumerable<Room>> GetAllByHotelAsync(int hotelId) =>
		await _context.Rooms.Include(r => r.Hotel).Where(r => r.HotelId == hotelId).ToListAsync();

	/// <inheritdoc/>
	public async Task<Room> UpdateAsync(Room room)
	{
		_context.Rooms.Update(room);
		await _context.SaveChangesAsync();
		return room;
	}
}
