using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelAgency2.Models
{
    public class Order
    {
        [Key]
        public int IdOrder { get; set; }
        public int IdTour { get; set; }
        public int IdEmployee { get; set; }
        public int IdClient { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int Cost { get; set; }

        public virtual Tour Tour { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Client Client { get; set; }
    }
}
