using Dapper;
using HotelBooking.Application.Dtos.Booking;
using HotelBooking.Application.Interfaces;
using MySql.Data.MySqlClient;

namespace HotelBooking.Infrastructure.Persistence.Repository;

/// <inheritdoc cref="IStatsRepository"/>
public class StatsRepository : IStatsRepository
{
	private readonly string _connectionString;

	public StatsRepository(string connectionString)
	{
		_connectionString = connectionString;
	}

	/// <inheritdoc/>
	public async Task<IEnumerable<HotelBookingStatsDto>> GetBookingsCountPerHotelAsync(DateTime? from = null, DateTime? to = null)
	{
		using var connection = new MySqlConnection(_connectionString);
		await connection.OpenAsync();

		var sql = @"
            SELECT 
                h.Id AS HotelId,
                h.Name AS HotelName,
                COUNT(b.Id) AS BookingCount
            FROM Hotels h
            LEFT JOIN Rooms r ON r.HotelId = h.Id
            LEFT JOIN Bookings b ON b.RoomId = r.Id
            WHERE (@From IS NULL OR b.CheckIn >= @From)
              AND (@To IS NULL OR b.CheckOut <= @To)
            GROUP BY h.Id, h.Name;
        ";

		var result = await connection.QueryAsync<HotelBookingStatsDto>(sql, new { From = from, To = to });
		return result;
	}
}
