using TravelAgencyApi.Data;
using TravelAgencyApi.Interfaces;
using TravelAgencyApi.Models;

namespace TravelAgencyApi.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.OrderBy(c => c.CountryName).ToList();
        }

        public Country GetCountry(int id)
        {
            return _context.Countries.Where(c => c.IdCountry == id).FirstOrDefault();
        }

        public int GetIdCountry(string countryName)
        {
            var country = _context.Countries.Where(c => c.CountryName == countryName).FirstOrDefault();

            if (country == null)
                return -1;

            return country.IdCountry;
        }
    }
}
