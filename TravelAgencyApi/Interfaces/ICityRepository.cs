using TravelAgencyApi.Models;

namespace TravelAgencyApi.Interfaces
{
    public interface ICityRepository
    {
        ICollection<City> GetCities(int idCountry);
        int GetIdCity(string cityName);
        City GetCity(int id);
    }
}
