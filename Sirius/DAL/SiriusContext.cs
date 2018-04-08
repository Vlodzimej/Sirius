using Microsoft.EntityFrameworkCore;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=SiriusDB;Username=sirius_user;Password=340571578;Integrated Security=false;");
        }
    }
}
