using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SS.Template.Services
{
    partial class AppDbContextInitializer
    {
        private async Task InitializeTerritories()
        {
            if (await _context.Regions.AnyAsync())
            {
                return;
            }

            var dataExtractor = new TerritoryDataExtractor(Path.Combine(_environment.ContentRootPath, "territories.txt"));
            dataExtractor.OnRegionExtracted = region => _context.Regions.Add(region);
            dataExtractor.OnSubregionExtracted = subregion => _context.Subregions.Add(subregion);
            dataExtractor.OnCountryExtracted = country => _context.Countries.Add(country);
            dataExtractor.Run();

            await _context.SaveChangesAsync();
        }
    }
}
