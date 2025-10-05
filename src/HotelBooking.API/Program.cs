using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Services;
using HotelBooking.Infrastructure.Persistence;
using HotelBooking.Infrastructure.Persistence.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlite("Data Source=hotelbooking.db");
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
	options.SignIn.RequireConfirmedAccount = false;
	options.SignIn.RequireConfirmedAccount = false;

	options.Password.RequireLowercase = false;
	options.Password.RequiredLength = 4;
	options.Password.RequireNonAlphanumeric = false;
})
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

builder.Services.AddScoped<HotelService>();
builder.Services.AddScoped<RoomService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
