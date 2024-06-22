using TravelAgencyApi.Data;
using TravelAgencyApi.Interfaces;
using TravelAgencyApi.Models;

namespace TravelAgencyApi.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly DataContext _context;

        public CityRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<City> GetCities(int idCountry)
        {
            return _context.Cities.Where(c => c.IdCountry == idCountry).OrderBy(c => c.CityName).ToList();
        }

        public City GetCity(int id)
        {
            return _context.Cities.Where(c => c.IdCity == id).FirstOrDefault();
        }

        public int GetIdCity(string cityName)
        {
            var city = _context.Cities.Where(c => c.CityName == cityName).FirstOrDefault();

            if (city == null)
                return -1;

            return city.IdCity;
        }
    }
}
