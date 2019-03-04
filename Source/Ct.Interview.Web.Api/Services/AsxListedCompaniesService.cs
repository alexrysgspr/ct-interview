using Ct.Interview.Web.Api.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using helper =  Ct.Interview.Web.Api.Helpers;

namespace Ct.Interview.Web.Api
{
    public class AsxListedCompaniesService : IAsxListedCompaniesService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        private readonly IMemoryCache _memoryCache;
        IStreamHelper streamHelper;
        public AsxListedCompaniesService(
            IConfiguration configuration,
            IMemoryCache memoryCache,
            HttpClient client)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
            _client = client;
        }

        public async Task<AsxListedCompany> GetByAsxCode(string asxCode)
        {
            var items = await FetchRecords();
            return items
                    .Where(m => m.AsxCode.ToLower() == asxCode.ToLower())
                    .SingleOrDefault();

        }
        public async Task<IEnumerable<AsxListedCompany>> GetAll() => await FetchRecords();


        private async Task<IEnumerable<AsxListedCompany>> FetchRecords()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            //Pure Code
            //streamHelper = new helper.StreamParser();
            //3rd Party
            streamHelper = new helper.CsvHelper();

            var now = DateTime.Now.Date.ToString("d");
            var exist = _memoryCache.TryGetValue(now.ToString(), out IEnumerable<AsxListedCompany> asxListedCompanies);
            if (!exist)
            {
                string uri = _configuration["AsxSettings:ListedSecuritiesCsvUrl"];
                var req = await _client.GetAsync(uri);
                asxListedCompanies = await streamHelper.ParseStream(await req.Content.ReadAsStreamAsync());
                var cacheEntryOptions = new MemoryCacheEntryOptions();
                _memoryCache.Set(now, asxListedCompanies, cacheEntryOptions);
            }

            stopwatch.Stop();
            Debug.WriteLine($"Completion Time : {stopwatch.ElapsedMilliseconds.ToString()}");
            return asxListedCompanies;
        }
    }
}
