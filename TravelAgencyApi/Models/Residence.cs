using System.ComponentModel.DataAnnotations;

namespace TravelAgencyApi.Models
{
    public class Residence
    {
        [Key]
        public int IdResidence { get; set; }
        public int IdCity { get; set; }
        public string? ResidenceName { get; set; }
        public string? Stars { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<Tour> Tours { get; set; }
    }
}
