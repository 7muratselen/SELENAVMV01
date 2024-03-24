using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SELENAVMV01.Models
{
    public class SELENAVVM01Context : DbContext
    {
        public SELENAVVM01Context(DbContextOptions<SELENAVVM01Context> options) : base(options)
        {
        }

        public DbSet<XML_Gumush_Product> Products { get; set; }
        public DbSet<XML_Gumush_Variant> Variants { get; set; }
        public DbSet<XML_Gumush_ProductImage> ProductImages { get; set; }
    }

    public class XML_Gumush_Product
    {
        // Özelliklerinizi buraya ekleyin

        
        public string Code { get; set; }
        [Key]
        public string WsCode { get; set; }
        public string Barcode { get; set; }
        public string SupplierCode { get; set; }
        public string Name { get; set; }
        public string ProductLink { get; set; }
        public string Cat1Name { get; set; }
        public string Cat1Code { get; set; }
        public string Cat2Name { get; set; }
        public string Cat2Code { get; set; }
        public string Cat3Name { get; set; }
        public string Cat3Code { get; set; }
        public string Cat4Name { get; set; } // Eklendi
        public string Cat4Code { get; set; } // Eklendi
        public string CategoryPath { get; set; }
        public int Stock { get; set; }
        public string Unit { get; set; }
        public decimal PriceList { get; set; }
        public decimal PriceListVatIncluded { get; set; }
        public decimal PriceListCampaign { get; set; }
        public decimal PriceSpecialVatIncluded { get; set; }
        public decimal PriceSpecial { get; set; }
        public int PriceSpecialRate { get; set; }
        public int? MinOrderQuantity { get; set; }
        public decimal PriceCreditCard { get; set; }
        public string Currency { get; set; }
        public int? Vat { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal Desi { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Deep { get; set; }
        public decimal Weight { get; set; }
        public string DeliveryDay { get; set; }
        public string Detail { get; set; }
        public List<XML_Gumush_Variant> Variants { get; set; } = new List<XML_Gumush_Variant>();
        public List<XML_Gumush_ProductImage> Images { get; set; } = new List<XML_Gumush_ProductImage>();
      
    }

    public class XML_Gumush_Variant
    {
        // Varyant özellikleri
        public int VaryantGroupID { get; set; }
        public string ProductCode { get; set; } // Ürün ile ilişkilendirmek için
        public string Code { get; set; }
        [Key]
        public string WsCode { get; set; }
        public string Type1 { get; set; }
        public string Type2 { get; set; } // XML'de bulunan ek alan
        public string Barcode { get; set; }
        public int Stock { get; set; }
        public decimal Desi { get; set; }
        public decimal PriceList { get; set; }
        public decimal PriceListDiscount { get; set; }
        public decimal PriceSpecial { get; set; }
        public string SupplierCode { get; set; }
    }

    public class XML_Gumush_ProductImage
    {
        [ForeignKey("Product")]
        public string ProductCode { get; set; }
        public string ImageUrl { get; set; }        
        public virtual XML_Gumush_Product Product { get; set; }
    }

   
}

