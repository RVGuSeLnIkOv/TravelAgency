using TravelAgencyApi.Data;
using TravelAgencyApi.Interfaces;
using TravelAgencyApi.Models;

namespace TravelAgencyApi.Repository
{
    public class ResidenceRepository : IResidenceRepository
    {
        private readonly DataContext _context;

        public ResidenceRepository(DataContext context)
        {
            _context = context;
        }

        public int GetIdResidence(string residenceName, string stars)
        {
            var residence = _context.Residences.Where(r => r.ResidenceName == residenceName && r.Stars == stars).FirstOrDefault();
            
            if (residence == null)
                return -1;

            return residence.IdResidence;
        }

        public Residence GetResidence(int id)
        {
            return _context.Residences.Where(r => r.IdResidence == id).FirstOrDefault();
        }

        public ICollection<Residence> GetResidences(int idCity)
        {
            return _context.Residences.Where(r => r.IdCity == idCity).OrderBy(r => r.ResidenceName).ToList();
        }
    }
}
