using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sirius.Helpers;
using Sirius.Models;

namespace Sirius.DAL
{
    public class SiriusContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Dimension> Dimensions { get; set; }

        public DbSet<Vendor> Vendors { get; set; }

        public DbSet<Register> Registers {get;set;}

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<InvoiceType> InvoiceTypes { get; set; }

        public DbSet<StorageRegister> StorageRegisters { get; set; }

        public DbSet<AccessLevel> AccessLevels { get; set; }

        public SiriusContext(DbContextOptions<SiriusContext> options) : base(options)  {  }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=SiriusDB;Username=sirius_user;Password=340571578;Integrated Security=false;");
            //optionsBuilder.UseNpgsql("Host=82.146.47.103;Port=5432;Database=siriusdb;Username=siriususer;Password=Uncle340571578;Integrated Security=false;");
        }
    }
}
 