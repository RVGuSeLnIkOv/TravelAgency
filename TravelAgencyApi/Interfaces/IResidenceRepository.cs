using TravelAgencyApi.Models;

namespace TravelAgencyApi.Interfaces
{
    public interface IResidenceRepository
    {
        ICollection<Residence> GetResidences(int idCity);
        int GetIdResidence(string residenceName, string stars);
        Residence GetResidence(int id);
    }
}
