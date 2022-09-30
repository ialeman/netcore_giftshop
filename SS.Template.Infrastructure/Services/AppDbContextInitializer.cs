using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SS.Template.Persistence;

namespace SS.Template.Services
{
    public partial class AppDbContextInitializer
    {
        private readonly AppDbContext _context;
        private readonly IHostEnvironment _environment;

        public AppDbContextInitializer(AppDbContext context, IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task Run()
        {
            //if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            //{
            //    await _context.Database.MigrateAsync();
            //}


            //if (!await _context.Examples.AnyAsync())
            //{
            //    _context.Examples.AddRange(
            //        new Example { Name = "Pasta", NormalizedName = "PASTA", Email = "pasta@food.com", Status = EnabledStatus.Enabled },
            //        new Example { Name = "Tacos", NormalizedName = "TACOS", Email = "tacos@food.com", Status = EnabledStatus.Enabled },
            //        new Example { Name = "Soup", NormalizedName = "SOUP", Email = "soup@food.com", Status = EnabledStatus.Enabled },
            //        new Example { Name = "Pizza", NormalizedName = "PIZZA", Email = "pizza@food.com", Status = EnabledStatus.Enabled },
            //        new Example { Name = "Burger", NormalizedName = "BURGER", Email = "burger@food.com", Status = EnabledStatus.Enabled },
            //        new Example { Name = "Salad", NormalizedName = "SALAD", Email = "salad@food.com", Status = EnabledStatus.Enabled }
            //    );

            //    await _context.SaveChangesAsync();
            //}

            await Migrate();

            await Seed();
        }

        private async Task Seed()
        {

            await InitializeTerritories();
        }

        private async Task Migrate()
        {
            if (_context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                await _context.Database.MigrateAsync();
            }
        }
    }
}
