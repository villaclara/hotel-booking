using HotelBooking.Application.Interfaces;
using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Persistence.Repository;

public class BookingRepository : IBookingRepository
{
	private readonly AppDbContext _context;

	public BookingRepository(AppDbContext context)
	{
		_context = context;
	}

	public async Task<Booking> AddAsync(Booking booking)
	{
		await _context.Bookings.AddAsync(booking);
		await _context.SaveChangesAsync();
		return booking;
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var booking = await _context.Bookings.FindAsync(id);
		if (booking == null)
		{
			return false;
		}
		_context.Bookings.Remove(booking);
		await _context.SaveChangesAsync();
		return true;
	}

	public async Task<Booking?> GetByIdAsync(int id) =>
		await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);

	public async Task<IEnumerable<Booking>> GetAllWithParamsAsync(string? userId = null)
	{
		var query = _context.Bookings.Include(b => b.Room).ThenInclude(r => r.Hotel).AsQueryable();
		if (userId is not null)
		{
			query = query.Where(b => b.UserId == userId);
		}

		return await query.ToListAsync();
	}

	public async Task<Booking> UpdateAsync(Booking booking)
	{
		_context.Bookings.Update(booking);
		await _context.SaveChangesAsync();
		return booking;
	}

	public async Task<bool> IsRoomAvailableAsync(int id, DateTime checkin, DateTime checkout)
	{
		return !await _context.Bookings
		.AnyAsync(b => b.RoomId == id &&
			b.CheckIn < checkout && checkin < b.CheckOut);
	}
}
