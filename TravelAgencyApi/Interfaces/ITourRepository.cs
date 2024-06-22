using TravelAgencyApi.Dto;
using TravelAgencyApi.Models;

namespace TravelAgencyApi.Interfaces
{
    public interface ITourRepository
    {
        ICollection<Tour> GetArchiveTours();
        ICollection<Tour> GetActiveTours();
        ICollection<Tourist> GetTouristsOnTour(int id);
        Tour GetTour(int id);
        int CreateTour(Tour tour, List<int> tourists);
        bool CheckingTour(Tour tour);
        bool CheckingTourists(List<int> tourists);
        bool UpdateTour(Tour tour);
        bool ArchiveTour(Tour tour);
        bool UnarchiveTour(Tour tour);
        bool TourExists(int id);
        bool Save();
    }
}
