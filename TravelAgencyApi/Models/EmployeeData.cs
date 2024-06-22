using System.ComponentModel.DataAnnotations;

namespace TravelAgencyApi.Models
{
    public class EmployeeData
    {
        [Key]
        public int IdEmployeeData { get; set; }
        public int IdEmployee { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
