using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelAgency2.Models
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
