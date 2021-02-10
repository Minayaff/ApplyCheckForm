using ApplyCheckESosial.Models;
using ApplyCheckESosial.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApplyCheckESosial.Extensions
{
    public class ESosialService : IESosialService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public ESosialService(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        public async Task<List<ESosialServiceData>> GetAsync(string idNumber, DateTime bithdate)
        {
            var date = bithdate.ToString("dd.MM.yyyy");
            var url = _configuration["ESosialData:ESosialServiceUrl"] + $"idNumber={idNumber}&birthDate={date}";
            var httpResponse = await _client.GetAsync(url);

            if (!httpResponse.IsSuccessStatusCode || httpResponse.Content == null)
            {
                return default;
            }

            var response = JsonConvert.DeserializeObject<List<ESosialServiceData>>(await httpResponse.Content.ReadAsStringAsync());

            if (response.Count == 0) return default;

            return response;
        }

        public async Task<List<ESosialServiceData>> GetAsync(string docNumber)
        {
            var url = _configuration["ESosialData:ESosialServiceUrl"] + $"number={docNumber}";
            var httpResponse = await _client.GetAsync(url);

            if (!httpResponse.IsSuccessStatusCode || httpResponse.Content == null)
            {
                return default;
            }

            var response = JsonConvert.DeserializeObject<ESosialServiceData>(await httpResponse.Content.ReadAsStringAsync());
            
            if (response == null) return default;

            List<ESosialServiceData> services = new List<ESosialServiceData>()
            {
                response
            };

            return services; 
        }
    }
}
