using TravelAgencyApi.Models;

namespace TravelAgencyApi.Interfaces
{
    public interface ITourOperatorRepository
    {
        ICollection<TourOperator> GetTourOperators();
        int GetIdTourOperator(string tourOperatorName);
        TourOperator GetTourOperator(int id);
    }
}
