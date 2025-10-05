using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext
{
	public DbSet<Hotel> Hotels { get; set; } = null!;
	public DbSet<Room> Rooms { get; set; } = null!;
	public DbSet<Booking> Bookings { get; set; } = null!;
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<IdentityUser>().ToTable("Users");    // idk about it.	
		builder.Entity<Hotel>().ToTable("Hotels");
		builder.Entity<Room>().ToTable("Rooms");
		builder.Entity<Booking>().ToTable("Bookings");

		builder.Entity<Hotel>(entity =>
		{
			entity.HasKey(h => h.Id);
			entity.Property(h => h.Name).IsRequired().HasMaxLength(200);
			entity.Property(h => h.Address).IsRequired().HasMaxLength(300);
			entity.Property(h => h.Description).HasMaxLength(1000);

			entity.HasMany(h => h.Rooms)
				  .WithOne(r => r.Hotel)
				  .HasForeignKey(r => r.HotelId)
				  .OnDelete(DeleteBehavior.Cascade);
		});

		builder.Entity<Room>(entity =>
		{
			entity.HasKey(r => r.Id);
			entity.Property(r => r.PricePerNight).HasPrecision(18, 2);
			entity.Property(r => r.Capacity).IsRequired();
		});

		builder.Entity<Booking>(entity =>
		{
			entity.HasKey(b => b.Id);

			entity.HasOne(b => b.Room)
				  .WithMany(r => r.Bookings)
				  .HasForeignKey(b => b.RoomId)
				  .OnDelete(DeleteBehavior.Cascade);

			entity.HasOne<IdentityUser>()
				  .WithMany()
				  .HasForeignKey(b => b.UserId)
				  .OnDelete(DeleteBehavior.Restrict);

			entity.Property(b => b.CheckIn).IsRequired();
			entity.Property(b => b.CheckOut).IsRequired();
		});

		builder.Entity<SimpleUser>(entity =>
		{
			entity.HasKey(su => su.Id);
			entity.Property(su => su.Email).IsRequired().HasMaxLength(200);
		});
	}
}
