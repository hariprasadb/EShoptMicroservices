using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System.Reflection;

namespace Ordering.Infrastructure.Data
{
    /*
        Add-Migration InitialCreate -OutputDir Data/Migrations  -Project Order-Infrastructure -StartupProject Ordering.API
    */
    public class ApplicationDBContext :DbContext ,IApplicationDBContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext>
                                    options) : base(options) 
        {
           
        }
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Customer>().Property(c => c.Name).IsRequired()
            //       .HasMaxLength(100);
            //IEntityTypeConfiguration
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());   
            base.OnModelCreating(modelBuilder);
        }
    }
}
