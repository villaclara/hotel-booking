using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Persistence.Repository;

public class HotelRepository : IHotelRepository
{
	private readonly AppDbContext _context;

	public HotelRepository(AppDbContext context)
	{
		_context = context;
	}

	public async Task<Hotel> AddAsync(Hotel hotel)
	{
		await _context.Hotels.AddAsync(hotel);
		await _context.SaveChangesAsync();
		return hotel;
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == id);
		if (hotel == null)
		{
			return false;
		}

		_context.Hotels.Remove(hotel);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<Hotel?> GetByIdAsync(int id)
	{
		return await _context.Hotels.Include(h => h.Rooms).FirstOrDefaultAsync(h => h.Id == id);
	}

	public async Task<IEnumerable<Hotel>> GetAllAsync() =>
		await _context.Hotels.Include(h => h.Rooms).ToListAsync();

	public async Task<Hotel> UpdateAsync(Hotel hotel)
	{
		_context.Hotels.Update(hotel);
		await _context.SaveChangesAsync();
		return hotel;
	}

	public async Task<bool> ExistsAsync(int id) =>
		await _context.Hotels.AnyAsync(h => h.Id == id);

	public async Task<IEnumerable<Hotel>> GetAllWithRoomsByDates(DateTime? checkin, DateTime? checkout, string? city)
	{
		var query = _context.Hotels.Include(h => h.Rooms).ThenInclude(r => r.Bookings).AsQueryable();

		if (!string.IsNullOrEmpty(city))
		{
			query = query.Where(h => h.Address.ToLower() == city.ToLower());
		}

		if (checkin.HasValue && checkout.HasValue)
		{
			query = query.Where(h =>
						h.Rooms.Any(r =>
							!r.Bookings.Any(b => b.CheckOut >= checkin && b.CheckIn <= checkout)));
		}

		return await query.ToListAsync();
	}
}
