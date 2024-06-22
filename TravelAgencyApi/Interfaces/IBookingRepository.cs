using TravelAgencyApi.Dto;
using TravelAgencyApi.Models;

namespace TravelAgencyApi.Interfaces
{
    public interface IBookingRepository
    {
        ICollection<Booking> GetBookings(int idTour);
        Booking GetBooking(int id);
        bool CreateBooking(Booking booking);
        bool CheckingBooking(Booking booking);
        bool UpdateBooking(Booking booking);
        bool DeleteBooking(Booking booking);
        bool BookingExists(int id);
        bool Save();
    }
}
