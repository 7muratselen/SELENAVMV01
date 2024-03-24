using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace SELENAVMV01.Models.Services
{
    public class XmlService
    {
        private readonly HttpClient _httpClient;

        public XmlService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Stream> GetXmlStreamAsync(string url)
        {
            var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStreamAsync();
        }
    }
}
