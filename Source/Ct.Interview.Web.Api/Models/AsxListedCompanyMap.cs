using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ct.Interview.Web.Api
{
    public class AsxListedCompanyMap : ClassMap<AsxListedCompany>
    {
        public AsxListedCompanyMap()
        {
            Map(m => m.CompanyName).Name("Company name");
            Map(m => m.AsxCode).Name("ASX code");
            Map(m => m.GicsIndustryGroup).Name("GICS industry group");
        }
    }
}
