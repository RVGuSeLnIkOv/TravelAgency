using System.ComponentModel.DataAnnotations;

namespace TravelAgencyApi.Models
{
    public class Tour
    {
        [Key]
        public int IdTour { get; set; }
        public int IdTourOperator { get; set; }
        public int IdResidence { get; set; }
        public int IdTypeMeal { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public int Duration { get; set; }
        public string? Notes { get; set; }
        public bool IsArchive { get; set; }

        public virtual Residence Residence { get; set; }
        public virtual TypeMeal TypeMeal { get; set; }
        public virtual TourOperator TourOperator { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<TouristTour> TouristsTours { get; set; }
    }
}
