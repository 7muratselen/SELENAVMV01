using Microsoft.EntityFrameworkCore;
using SELENAVMV01.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SELENAVMV01.Models.Services
{
    public class ProductService
    {
        private readonly SELENAVVM01Context _context;

        public ProductService(SELENAVVM01Context context)
        {
            _context = context;
        }

        public async Task TruncateAndProcessProductsAsync(List<XML_Gumush_Product> products)
        {
            await TruncateTablesAsync();
            await ProcessProductsAsync(products);
        }

        private async Task TruncateTablesAsync()
        {
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE XML_Gumush_ProductImages");
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE XML_Gumush_Variants");
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE XML_Gumush_Products");
        }

        private async Task ProcessProductsAsync(List<XML_Gumush_Product> products)
        {
            foreach (var product in products)
            {
                _context.Products.Add(product);
                foreach (var variant in product.Variants)
                {
                    _context.Variants.Add(variant);
                }
                foreach (var image in product.Images)
                {
                    _context.ProductImages.Add(image);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
