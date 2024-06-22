using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace TravelAgencyApi.Models
{
    public class Booking
    {
        [Key]
        public int IdBooking { get; set; }
        public int IdTour { get; set; }
        public int IdTourist { get; set; }
        public int IdEmployee { get; set; }
        public DateTime? Date { get; set; }
        public int? Cost { get; set; }

        public virtual Tour Tour { get; set; }
        public virtual Tourist Tourist { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
