using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelAgency2.Models
{
    public class Client
    {
        [Key]
        public int IdClient { get; set; }
        public string ClientSurname { get; set; }
        public string ClientName { get; set; }
        public string ClientPatronymic { get; set; }
        public DateTime ClientBirthday { get; set; }
        public string ClientGender { get; set; }
        public string ClientAddress { get; set; }
        public string ClientPhoneNumber { get; set; }

        public virtual ClientData ClientData { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}
