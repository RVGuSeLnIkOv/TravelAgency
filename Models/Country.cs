using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelAgency2.Models
{
    public class Country
    {
        [Key]
        public int IdCountry { get; set; }
        public string CountryName { get; set; }

        public virtual ICollection<City> City { get; set; }
    }
}
