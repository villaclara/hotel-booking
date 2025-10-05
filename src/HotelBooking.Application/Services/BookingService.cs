using HotelBooking.Application.Interfaces;

namespace HotelBooking.Application.Services;

public class BookingService
{
	private readonly IBookingRepository _bookingRepository;

	public BookingService(IBookingRepository bookingRepository)
	{
		_bookingRepository = bookingRepository;
	}

	public void geal()
	{
		_bookingRepository.GetAllWithParamsAsync();
	}
}
