using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
            context.Database.MigrateAsync().GetAwaiter().GetResult();
            await SeedAsync(context);
        }
        private static async Task SeedAsync(ApplicationDBContext context)
        {
            await SeedCustomerAsync(context);
            await SeedProductAsync(context);
            await SeedOrdersWithItemAsync(context);
        }
        private static async Task SeedCustomerAsync(ApplicationDBContext context)
        {
            if (!await context.Customers.AnyAsync())
            {
                await context.Customers.AddRangeAsync(InitialData.Customers);
                await context.SaveChangesAsync();
            }
        }
        private static async Task SeedProductAsync(ApplicationDBContext context)
        {
            if (!await context.Products.AnyAsync())
            {
                await context.Products.AddRangeAsync(InitialData.Products);
                await context.SaveChangesAsync();
            }
        }
        private static async Task SeedOrdersWithItemAsync(ApplicationDBContext context)
        {
            try
            {
                if (!await context.Orders.AnyAsync())
                {
                    await context.Orders.AddRangeAsync(InitialData.OrdersWithItems);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                 Console.WriteLine(ex.Message);
            }
        }
    }
}
