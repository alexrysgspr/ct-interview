using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ct.Interview.Web.Api.Abstractions
{
    interface IStreamHelper
    {
        Task<IEnumerable<AsxListedCompany>> ParseStream(Stream stream);
    }
}
