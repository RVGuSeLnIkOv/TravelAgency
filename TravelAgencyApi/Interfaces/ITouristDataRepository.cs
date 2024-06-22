using Microsoft.AspNetCore.Mvc.ModelBinding;
using TravelAgencyApi.Dto;
using TravelAgencyApi.Models;

namespace TravelAgencyApi.Interfaces
{
    public interface ITouristDataRepository
    {
        bool CreateTouristData(TouristData touristData);
        bool CheckingTouristData(TouristData touristData);
        bool UpdateTouristData(TouristData touristData);
        bool Save();
        bool TouristDataExists(int id);
        bool TouristHaveData(int idTourist);
    }
}
