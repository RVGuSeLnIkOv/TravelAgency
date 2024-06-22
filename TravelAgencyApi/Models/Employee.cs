using System.ComponentModel.DataAnnotations;

namespace TravelAgencyApi.Models
{
    public class Employee
    {
        [Key]
        public int IdEmployee { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string? Patronymic { get; set; }
        public DateTime BirthDate { get; set; }
        public int? Salary { get; set; }
        public string Post { get; set; }
        public string? PhoneNumber { get; set; }

        public virtual EmployeeData EmployeeData { get; set; }
        public virtual ICollection<Booking> Bookings { get;}
    }
}
