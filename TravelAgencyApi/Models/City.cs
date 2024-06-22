using System.ComponentModel.DataAnnotations;

namespace TravelAgencyApi.Models
{
    public class City
    {
        [Key]
        public int IdCity { get; set; }
        public int IdCountry { get; set; }
        public string? CityName { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<Residence> Residences { get; set; }
    }
}
