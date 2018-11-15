using EFPlayground.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFPlayground.Data
{
    public class EFPlaygroundContext : DbContext, IDataContext
    {
        public EFPlaygroundContext()
            : base("EFPlayground")
        {
            base.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Group> Groups { get; set; }
    }
}
