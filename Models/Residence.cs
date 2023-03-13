using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelAgency2.Models
{
    public class Residence
    {
        [Key]
        public int IdResidence { get; set; }
        public int IdCity { get; set; }
        public int IdTypeResidence { get; set; }
        public string ResidenceName { get; set; }
        public int Stars { get; set; }

        public virtual City City { get; set; }

        public virtual TypeResidence TypeResidence { get; set; }

        public virtual ICollection<Tour> Tour { get; set; }
    }
}
