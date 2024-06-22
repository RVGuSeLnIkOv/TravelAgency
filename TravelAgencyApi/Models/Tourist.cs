using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyApi.Models
{
    public class Tourist
    {
        [Key]
        public int IdTourist { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string? Patronymic { get; set; }
        public string? ForeignSurname { get; set; }
        public string? ForeignName { get; set; }
        public string? ForeignPatronymic { get; set; }
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public bool IsChild { get; set; }

        public virtual TouristData TouristData { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<TouristTour> TouristsTours { get; set; }
    }
}
