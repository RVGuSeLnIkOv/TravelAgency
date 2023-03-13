using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelAgency2.Models
{
    public class Employee
    {
        [Key]
        public int IdEmployee { get; set; }
        public string EmployeeSurname { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePatronymic { get; set; }
        public DateTime EmployeeBirthday { get; set; }
        public int Salary { get; set; }
        public string Post { get; set; }
        public string EmployeePhoneNumber { get; set; }

        public virtual EmployeeData EmployeeData { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}
