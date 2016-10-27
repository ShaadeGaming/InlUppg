using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlamningsuppgift1_Dbt
{
   public class Contact
    {
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; } 
        public string Zip { get; set; } 
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime Birthday{ get; set; }

    }
}
