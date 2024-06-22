using TravelAgencyApi.Data;
using TravelAgencyApi.Interfaces;
using TravelAgencyApi.Models;

namespace TravelAgencyApi.Repository
{
    public class TourOperatorRepository : ITourOperatorRepository
    {
        private readonly DataContext _context;

        public TourOperatorRepository(DataContext context)
        {
            _context = context;
        }

        public int GetIdTourOperator(string tourOperatorName)
        {
            var tourOperator = _context.TourOperators.Where(to => to.TourOperatorName == tourOperatorName).FirstOrDefault();

            if (tourOperator == null)
                return -1;

            return tourOperator.IdTourOperator;
        }

        public TourOperator GetTourOperator(int id)
        {
            return _context.TourOperators.Where(to => to.IdTourOperator == id).FirstOrDefault();
        }

        public ICollection<TourOperator> GetTourOperators()
        {
            return _context.TourOperators.OrderBy(to => to.TourOperatorName).ToList();
        }
    }
}
