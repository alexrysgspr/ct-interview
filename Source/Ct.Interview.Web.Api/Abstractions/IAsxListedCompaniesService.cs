using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ct.Interview.Web.Api
{
    public interface IAsxListedCompaniesService
    {
        Task<AsxListedCompany> GetByAsxCode(string asxCode);
        Task<IEnumerable<AsxListedCompany>> GetAll();
    }
}
