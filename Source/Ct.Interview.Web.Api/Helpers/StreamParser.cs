using Ct.Interview.Web.Api.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace Ct.Interview.Web.Api.Helpers
{
    public class StreamParser : IStreamHelper
    {
        public async Task<IEnumerable<AsxListedCompany>> ParseStream(Stream stream)
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
    }
}
