using Microsoft.EntityFrameworkCore;
using TravelAgencyApi.Data;
using TravelAgencyApi.Dto;
using TravelAgencyApi.Helper;
using TravelAgencyApi.Interfaces;
using TravelAgencyApi.Models;

namespace TravelAgencyApi.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DataContext _context;

        public BookingRepository(DataContext context)
        {
            _context = context;
        }

        public bool BookingExists(int id)
        {
            return _context.Bookings.Any(b => b.IdBooking == id);
        }

        public bool CheckingBooking(Booking booking)
        {
            if (!string.IsNullOrEmpty(booking.Date.ToString())
                && booking.Date != DateTime.MinValue
                && Checking.CheckingInt(booking.Cost.ToString(), 0, Int32.MaxValue) != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CreateBooking(Booking booking)
        {
            _context.Add(booking);

            return Save();
        }

        public bool DeleteBooking(Booking booking)
        {
            _context.Remove(booking);
            return Save();
        }

        public Booking GetBooking(int id)
        {
            return _context.Bookings.Where(b => b.IdBooking == id).FirstOrDefault();
        }

        public ICollection<Booking> GetBookings(int idTour)
        {
            return _context.Bookings.Where(b => b.IdTour == idTour).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateBooking(Booking booking)
        {
            _context.Update(booking);

            return Save();
        }
    }
}
