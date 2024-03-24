using System.Xml;
using Microsoft.EntityFrameworkCore;
using SELENAVMV01.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SELENAVMV01.Models.Services
{
    public class XmlParsingService
    {
        private readonly SELENAVVM01Context _context;

        public XmlParsingService(SELENAVVM01Context context)
        {
            _context = context;
        }

        public async Task ParseXmlAsync(Stream xmlStream)
        {
            using (var reader = XmlReader.Create(xmlStream, new XmlReaderSettings { Async = true }))
            {
                XML_Gumush_Product product = null;
                List<XML_Gumush_Variant> variants = null;
                List<XML_Gumush_ProductImage> images = null;

                while (await reader.ReadAsync())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (reader.Name)
                            {
                                case "product":
                                    product = await FindOrCreateProductAsync(reader);
                                    variants = new List<XML_Gumush_Variant>();
                                    images = new List<XML_Gumush_ProductImage>();
                                    break;

                                case "subproduct":
                                    if (product != null)
                                    {
                                        var variant = await ReadVariantElement(reader);
                                        variants.Add(variant);
                                    }
                                    break;

                                case "img_item":
                                    if (product != null)
                                    {
                                        var image = await ReadImageElement(reader);
                                        images.Add(image);
                                    }
                                    break;

                                default:
                                    if (product != null)
                                    {
                                        await ReadProductElement(reader, product);
                                    }
                                    break;
                            }
                            break;

                        case XmlNodeType.EndElement:
                            if (reader.Name == "product")
                            {
                                product.Variants = variants;
                                product.Images = images;
                                _context.Products.Update(product); // Mevcut nesneyi günceller
                            }
                            break;
                    }
                }
                await _context.SaveChangesAsync();
            }
        }

        private async Task<XML_Gumush_Product> FindOrCreateProductAsync(XmlReader reader)
        {
            var wsCode = await reader.ReadElementContentAsStringAsync("ws_code");

            var product = await _context.Products.FirstOrDefaultAsync(p => p.WsCode == wsCode);
            if (product == null)
            {
                product = new XML_Gumush_Product();
                await ReadProductElement(reader, product); // Yeni ürün için bilgileri oku
            }
            return product;
        }

        private async Task<XML_Gumush_Variant> ReadVariantElement(XmlReader reader)
        {
            var variant = new XML_Gumush_Variant();

            while (await reader.ReadAsync())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.Name)
                        {
                            case "VaryantGroupID":
                                variant.VaryantGroupID = int.Parse(await reader.ReadElementContentAsStringAsync());
                                break;
                            case "code":
                                variant.Code = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "ws_code":
                                variant.WsCode = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "type1":
                                variant.Type1 = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "type2":
                                variant.Type2 = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "barcode":
                                variant.Barcode = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "stock":
                                variant.Stock = int.Parse(await reader.ReadElementContentAsStringAsync());
                                break;
                            case "desi":
                                variant.Desi = decimal.Parse(await reader.ReadElementContentAsStringAsync());
                                break;
                            case "price_list":
                                variant.PriceList = decimal.Parse(await reader.ReadElementContentAsStringAsync());
                                break;
                                variant.PriceListDiscount = decimal.Parse(await reader.ReadElementContentAsStringAsync());
                                break;
                            case "price_special":
                                variant.PriceSpecial = decimal.Parse(await reader.ReadElementContentAsStringAsync());
                                break;
                            case "supplier_code":
                                variant.SupplierCode = await reader.ReadElementContentAsStringAsync();
                                break;
                        }
                        break;
                }
            }

            return variant;
        }

        private async Task<XML_Gumush_ProductImage> ReadImageElement(XmlReader reader)
        {
            var image = new XML_Gumush_ProductImage();

            while (await reader.ReadAsync())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.Name)
                        {
                            case "image_url":
                                image.ImageUrl = await reader.ReadElementContentAsStringAsync();
                                break;
                        }
                        break;
                }
            }

            return image;
        }

        private async Task ReadProductElement(XmlReader reader, XML_Gumush_Product product)
        {
            while (await reader.ReadAsync())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.Name)
                        {
                            case "code":
                                product.Code = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "ws_code":
                                product.WsCode = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "barcode":
                                product.Barcode = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "supplier_code":
                                product.SupplierCode = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "name":
                                product.Name = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "product_link":
                                product.ProductLink = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "cat1_name":
                                product.Cat1Name = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "cat1_code":
                                product.Cat1Code = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "cat2_name":
                                product.Cat2Name = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "cat2_code":
                                product.Cat2Code = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "cat3_name":
                                product.Cat3Name = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "cat3_code":
                                product.Cat3Code = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "cat4_name":
                                product.Cat4Name = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "cat4_code":
                                product.Cat4Code = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "category_path":
                                product.CategoryPath = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "stock":
                                product.Stock = int.Parse(await reader.ReadElementContentAsStringAsync());
                                break;
                            case "unit":
                                product.Unit = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "price_list":
                                product.PriceList = decimal.Parse(await reader.ReadElementContentAsStringAsync());
                                break;
                            case "price_list_vat_included":
                                product.PriceListVatIncluded = decimal.Parse(await reader.ReadElementContentAsStringAsync());
                                break;
                            case "price_list_campaign":
                                product.PriceListCampaign = decimal.Parse(await reader.ReadElementContentAsStringAsync());
                                break;
                            case "price_special_vat_included":
                                product.PriceSpecialVatIncluded = decimal.Parse(await reader.ReadElementContentAsStringAsync());
                                break;
                            case "price_special":
                                product.PriceSpecial = decimal.Parse(await reader.ReadElementContentAsStringAsync());
                                break;
                            case "price_special_rate":
                                product.PriceSpecialRate = int.Parse(await reader.ReadElementContentAsStringAsync());
                                break;
                            case "min_order_quantity":
                                if (!string.IsNullOrEmpty(await reader.ReadElementContentAsStringAsync()))
                                    {
                                        product.MinOrderQuantity = int.Parse(await reader.ReadElementContentAsStringAsync());
                                    }
                                break;
                            case "price_credit_card":
                                product.PriceCreditCard = decimal.Parse(await reader.ReadElementContentAsStringAsync());
                                break;
                            case "currency":
                                product.Currency = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "vat":
                                if (!string.IsNullOrEmpty(await reader.ReadElementContentAsStringAsync()))
                                {
                                    product.Vat = int.Parse(await reader.ReadElementContentAsStringAsync());
                                }
                                break;
                            case "brand":
                                product.Brand = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "model":
                                product.Model = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "desi":
                                product.Desi = decimal.Parse(await reader.ReadElementContentAsStringAsync());
                                break;
                            case "width":
                                product.Width = decimal.Parse(await reader.ReadElementContentAsStringAsync());
                                break;
                            case "height":
                                product.Height = decimal.Parse(await reader.ReadElementContentAsStringAsync());
                                break;
                            case "deep":
                                product.Deep = decimal.Parse(await reader.ReadElementContentAsStringAsync());
                                break;
                            case "weight":
                                product.Weight = decimal.Parse(await reader.ReadElementContentAsStringAsync());
                                break;
                            case "delivery_day":
                                product.DeliveryDay = await reader.ReadElementContentAsStringAsync();
                                break;
                            case "detail":
                                product.Detail = await reader.ReadElementContentAsStringAsync();
                                break;
                        }
                        break;
                }
            }
        }
    }
}





