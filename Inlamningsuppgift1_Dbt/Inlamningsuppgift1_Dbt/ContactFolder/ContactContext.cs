using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inlamningsuppgift1_Dbt
{
    public class ContactContext : DbContext 
    {
        public DbSet<Contact> Contacts { get; set; }

    }
}
