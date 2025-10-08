using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Persistence;

public static class Seed
{
	public static async Task SeedAsync(AppDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
	{
		// Ensure database exists
		await context.Database.EnsureCreatedAsync();

		// Seed Roles
		string[] roles = ["Admin", "User"];
		foreach (var role in roles)
		{
			if (!await roleManager.RoleExistsAsync(role))
			{
				await roleManager.CreateAsync(new IdentityRole(role));
			}
		}

		// Seed Users
		if (!context.Users.Any())
		{
			var users = new List<IdentityUser>
			{
				new() { UserName = "admin@example.com", Email = "admin@example.com" },
				new() { UserName = "user1@example.com", Email = "user1@example.com" },
				new() { UserName = "user2@example.com", Email = "user2@example.com" },
				new() { UserName = "user3@example.com", Email = "user3@example.com" },
				new() { UserName = "user4@example.com", Email = "user4@example.com" }
			};

			foreach (var user in users)
			{
				await userManager.CreateAsync(user, "123123");
				await userManager.AddToRoleAsync(user, user.UserName == "admin@example.com" ? "Admin" : "User");
			}
		}

		// Seed Hotels
		if (!context.Hotels.Any())
		{
			var hotels = new List<Hotel>
			{
				new() { Name = "Hotel California", Address = "42 Sunset Blvd", Description = "A lovely place." },
				new() { Name = "Grand Budapest", Address = "1 Alpine St", Description = "Famous for luxury." },
				new() { Name = "Hilton Downtown", Address = "100 City Ave", Description = "Modern hotel in city center." },
				new() { Name = "Sea View Resort", Address = "7 Ocean Drive", Description = "Beachside resort." },
				new() { Name = "Mountain Lodge", Address = "99 Hilltop Rd", Description = "Cozy mountain hotel." }
			};

			context.Hotels.AddRange(hotels);
			await context.SaveChangesAsync();
		}

		// Seed Rooms
		if (!context.Rooms.Any())
		{
			var hotels = await context.Hotels.ToListAsync();
			var random = new Random();
			var rooms = new List<Room>();

			foreach (var hotel in hotels)
			{
				for (int i = 1; i <= 3; i++) // 3 rooms per hotel
				{
					rooms.Add(new Room
					{
						HotelId = hotel.Id,
						Description = $"Room {i} in {hotel.Name}",
						Capacity = random.Next(1, 5),
						PricePerNight = random.Next(80, 300)
					});
				}
			}

			context.Rooms.AddRange(rooms);
			await context.SaveChangesAsync();
		}

		// Seed Bookings
		if (!context.Bookings.Any())
		{
			var rooms = await context.Rooms.ToListAsync();
			var usersList = await userManager.Users.ToListAsync();

			if (!rooms.Any()) return; // ensure rooms exist

			var random = new Random();
			var bookings = new List<Booking>();

			for (int i = 0; i < 10; i++)
			{
				var room = rooms[random.Next(rooms.Count)];
				var user = usersList[random.Next(usersList.Count)];

				var checkIn = DateTime.Today.AddDays(random.Next(1, 30));
				var checkOut = checkIn.AddDays(random.Next(1, 5));

				bookings.Add(new Booking
				{
					RoomId = room.Id,
					UserId = user.Id,
					CheckIn = checkIn,
					CheckOut = checkOut
				});
			}

			context.Bookings.AddRange(bookings);
			await context.SaveChangesAsync();
		}
	}
}
