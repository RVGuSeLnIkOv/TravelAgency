using System.ComponentModel.DataAnnotations;

namespace TravelAgencyApi.Models
{
    public class TourOperator
    {
        [Key]
        public int IdTourOperator { get; set; }
        public string? TourOperatorName { get; set; }

        public virtual ICollection<Tour> Tours { get; set; }
    }
}
