using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using SELENAVMV01.Models;
using SELENAVMV01.Models.Services;

var builder = WebApplication.CreateBuilder(args);

// HttpClient ve servislerinizi kaydedin
builder.Services.AddHttpClient<XmlService>();
builder.Services.AddScoped<XmlParser>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddDbContext<SELENAVVM01Context>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("SELENAVVM01Database")));


var app = builder.Build();

// HTTP GET isteði üzerinde asenkron iþlemleri yapacak bir endpoint oluþturun
app.MapGet("/", async (XmlService xmlService, XmlParser xmlParser, ProductService productService) =>
{
    var xmlData = await xmlService.GetXmlAsync("https://www.gumush.com/xml/?R=6575&K=bfb9&AltUrun=1&TamLink=1&Dislink=1&Imgs=1&start=0&limit=99999&pass=8RRqo6ZF&nocache&currency=TL&Stok=1");
    var products = xmlParser.ParseXml(xmlData);
    await productService.TruncateAndProcessProductsAsync(products);

    return Results.Ok(new { Message = "Products processed successfully" });
});

app.Run();
