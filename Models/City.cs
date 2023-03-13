using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelAgency2.Models
{
    public class City
    {
        [Key]
        public int IdCity { get; set; }
        public int IdCountry { get; set; }
        public string CityName { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<Residence> Residence { get; set; }

        public virtual ICollection<Tour> Tour { get; set; }
    }
}
