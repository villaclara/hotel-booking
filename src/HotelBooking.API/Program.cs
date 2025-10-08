using HotelBooking.Application.Interfaces;
using HotelBooking.Application.Services;
using HotelBooking.Infrastructure.Persistence;
using HotelBooking.Infrastructure.Persistence.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SQLITE FOR TESTING
//builder.Services.AddDbContext<AppDbContext>(options =>
//{
//	options.UseSqlite("Data Source=hotelbooking.db");
//});

// MYSQL
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
		ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
	options.SignIn.RequireConfirmedAccount = false;
	options.SignIn.RequireConfirmedAccount = false;

	options.Password.RequireUppercase = false;
	options.Password.RequireLowercase = false;
	options.Password.RequiredLength = 4;
	options.Password.RequireNonAlphanumeric = false;
})
	.AddEntityFrameworkStores<AppDbContext>()
	.AddDefaultTokenProviders();

// Configuring the response for the API controllers to not return html login page.
// However it would be better to use JWT in controllers, and Cookie in Razor Pages.
builder.Services.ConfigureApplicationCookie(options =>
{
	options.Events.OnRedirectToLogin = context =>
	{
		if (context.Request.Path.StartsWithSegments("/api"))
		{
			context.Response.StatusCode = 401;
			return Task.CompletedTask;
		}
		context.Response.Redirect(context.RedirectUri);
		return Task.CompletedTask;
	};

	options.Events.OnRedirectToAccessDenied = context =>
	{
		if (context.Request.Path.StartsWithSegments("/api"))
		{
			context.Response.StatusCode = 403;
			return Task.CompletedTask;
		}
		context.Response.Redirect(context.RedirectUri);
		return Task.CompletedTask;
	};
});


builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IStatsRepository, StatsRepository>(sp =>
{
	var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
	return new StatsRepository(connectionString!);
});

builder.Services.AddScoped<HotelService>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<StatsService>();

var app = builder.Build();

// Creating Roles and Seeding Admin data.
using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


	await context.Database.EnsureDeletedAsync();
	//await context.Database.EnsureCreatedAsync();
	await context.Database.MigrateAsync();

	await Seed.SeedAsync(context, userManager, roleManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
