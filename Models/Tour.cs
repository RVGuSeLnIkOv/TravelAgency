using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelAgency2.Models
{
    public class Tour
    {
        [Key]
        public int IdTour { get; set; }
        public int IdCity { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public int IdResidence { get; set; }
        public int Duration { get; set; }
        public int AdultsNumber { get; set; }
        public int ChildrenNumber { get; set; }

        public virtual City City { get; set; }

        public virtual Residence Residence { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}
