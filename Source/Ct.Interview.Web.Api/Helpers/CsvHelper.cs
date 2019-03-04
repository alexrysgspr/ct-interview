using CsvHelper;
using Ct.Interview.Web.Api.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ct.Interview.Web.Api.Helpers
{
    public class CsvHelper : IStreamHelper
    {
        public async Task<IEnumerable<AsxListedCompany>> ParseStream(Stream stream)
        {
            var sr = new StreamReader(stream);
            using (CsvReader csv = new CsvReader(sr))
            {
                await csv.ReadAsync();
                await csv.ReadAsync();
                csv.ReadHeader();
                csv.Configuration.RegisterClassMap<AsxListedCompanyMap>();
                return csv
                        .GetRecords<AsxListedCompany>().ToList();
            }
        }
    }
}
