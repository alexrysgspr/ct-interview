using CsvHelper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ct.Interview.Web.Api
{
    public class AsxListedCompaniesService : IAsxListedCompaniesService
    {
        private readonly IConfiguration configuration;
        private readonly IMemoryCache _memoryCache;

        public AsxListedCompaniesService(
            IConfiguration Configuration,
            IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            configuration = Configuration;
        }

        #region With 3rd party library
        //public async Task<AsxListedCompany> GetByAsxCode(string asxCode)
        //{
        //    string uri = configuration["AsxSettings:ListedSecuritiesCsvUrl"];
        //    using (var client = new HttpClient())
        //    {
        //        var req = await client.GetAsync(uri);
        //        StreamReader sr = new StreamReader(await req.Content.ReadAsStreamAsync());
        //        var company = GetAsxListedCompanyFromStream(sr);
        //        sr.Close();
        //        return company
        //                .Where(s => s.AsxCode.ToLower() == asxCode.ToLower())
        //                .SingleOrDefault();
        //    }
        //}
        //private IEnumerable<AsxListedCompany> GetAsxListedCompanyFromStream(StreamReader sr)
        //{
        //    using (CsvReader csv = new CsvReader(sr))
        //    {
        //        csv.Read();
        //        csv.Read();
        //        csv.ReadHeader();
        //        csv.Configuration.RegisterClassMap<AsxListedCompanyMap>();
        //        return csv
        //                .GetRecords<AsxListedCompany>().ToList();
        //    }
        //}
        #endregion
        #region Pure code
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
            var now = DateTime.Now.Date.ToString("d");
            IEnumerable<AsxListedCompany> asxListedCompanies;
            var exist = _memoryCache.TryGetValue(now.ToString(), out asxListedCompanies);
            if (!exist)
            {
                string uri = configuration["AsxSettings:ListedSecuritiesCsvUrl"];
                using (var client = new HttpClient())
                {
                    var req = await client.GetAsync(uri);
                    asxListedCompanies = await StreamToListAsync(await req.Content.ReadAsStreamAsync());
                }
                var cacheEntryOptions = new MemoryCacheEntryOptions();
                _memoryCache.Set(now, asxListedCompanies, cacheEntryOptions);
            }
            return asxListedCompanies;
        }
        private async Task<IEnumerable<AsxListedCompany>> StreamToListAsync(Stream stream)
        {
            var sr = new StreamReader(stream);
            var list = new List<AsxListedCompany>();
            string companyLine;
            await sr.ReadLineAsync();
            await sr.ReadLineAsync();
            while ((companyLine = await sr.ReadLineAsync()) != null)
            {
                var columns = companyLine.Split(",");
                var company = new AsxListedCompany
                {
                    AsxCode = Regex.Unescape(columns[1]).Replace("\"", ""),
                    CompanyName = Regex.Unescape(columns[0]).Replace("\"", ""),
                    GicsIndustryGroup = Regex.Unescape(columns[2]).Replace("\"", "")
                };
                list.Add(company);
            }
            sr.Close();
            return list;
        }
        #endregion
    }
}
