using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace otus_interfaces
{
    internal class ExchangeRatesApiConverter : ICurrencyConverter
    {
        private HttpClient _httpClient;
        private MemoryCache _memoryCache;
        private string _apiKey;

        public ExchangeRatesApiConverter(HttpClient httpClient, MemoryCache memoryCache, string apiKey)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
            _apiKey = apiKey;
        }

        public ICurrencyAmount ConvertCurrency(ICurrencyAmount amount, string currencyCode)
        {
            return ConvertCurrencyAsync(amount, currencyCode).Result;
        }

        private async Task<ExchangeRatesApiResponse> GetExchangeRatesAsync()
        {
            //Trace.TraceInformation("Request to exchangeratesapi is sending");
            var response = await _httpClient.GetAsync($"http://api.exchangeratesapi.io/v1/latest?access_key={_apiKey}");
            //Trace.TraceInformation("Response from exchangeratesapi is received");
            response = response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ExchangeRatesApiResponse>(json);
        }

        public class ExchangeRatesApiResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("timestamp")]
            public long Timestamp { get; set; }

            [JsonProperty("base")]
            public string Base { get; set; }

            [JsonProperty("date")]
            public DateTimeOffset Date { get; set; }

            [JsonProperty("rates")]
            public Dictionary<string, double> Rates { get; set; }
        }

        public async Task<ICurrencyAmount> ConvertCurrencyAsync(ICurrencyAmount money, string targetCurrencyCode)
        {
            var cashedResponse = await _memoryCache.GetOrCreateAsync("response", ce =>
            {
                ce.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);
                return GetExchangeRatesAsync();
            });

            var AmountInBase = money.Amount / (decimal)cashedResponse.Rates[money.CurrencyCode];
            var targetAmount = AmountInBase * (decimal)cashedResponse.Rates[targetCurrencyCode];

            return new CurrencyAmount(targetCurrencyCode, targetAmount);
        }
    }
}