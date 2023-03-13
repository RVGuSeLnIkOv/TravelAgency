using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelAgency2.Models
{
    public class ClientData
    {
        [Key]
        public int IdClientData { get; set; }
        public int IdClient { get; set; }
        public string InternalPassportNumber { get; set; }
        public string ForeignPassportNumber { get; set; }

        public virtual Client Client { get; set; }
    }
}
