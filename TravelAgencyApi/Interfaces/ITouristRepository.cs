using TravelAgencyApi.Dto;
using TravelAgencyApi.Models;

namespace TravelAgencyApi.Interfaces
{
    public interface ITouristRepository
    {
        ICollection<Tourist> GetTourists();
        ICollection<Tourist> GetAdultTourists();
        ICollection<Tourist> GetChildrenTourists();
        ICollection<Tourist> GetTouristsSearch
            (string? surname = null,
            string? name = null,
            string? patronymic = null,
            DateTime? birthDate = null);
        Tourist GetTourist(int id);
        Tourist GetTouristTrimToUpper(TouristDto touristDto);
        TouristData GetTouristData(int id);
        int CreateTourist(Tourist tourist);
        bool CheckingTourist(Tourist tourist);
        bool UpdateTourist(Tourist tourist);
        bool DeleteTourist(Tourist tourist);
        bool TouristExists(int id);
        bool Save();
    }
}
