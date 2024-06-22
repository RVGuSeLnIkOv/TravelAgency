using System.ComponentModel.DataAnnotations;

namespace TravelAgencyApi.Models
{
    public class TouristData
    {
        [Key]
        public int IdTouristData { get; set; }
        public int IdTourist { get; set; }
        public string? Address { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public string? InternalPassportNumber { get; set; }
        public string? ForeignPassportNumber { get; set; }
        public string? BirthCertificateNumber { get; set; }

        public virtual Tourist Tourist { get; set; }
    }
}
