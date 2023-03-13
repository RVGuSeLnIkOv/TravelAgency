using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelAgency2.Models
{
    public class TypeResidence
    {
        [Key]
        public int IdTypeResidence { get; set; }
        public string TypeResidenceName { get; set; }

        public virtual ICollection<Residence> Residence { get; set; }
    }
}
