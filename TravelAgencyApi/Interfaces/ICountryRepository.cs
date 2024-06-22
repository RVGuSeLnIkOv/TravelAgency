using TravelAgencyApi.Models;

namespace TravelAgencyApi.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        int GetIdCountry(string countryName);
        Country GetCountry(int id);
    }
}
