using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ct.Interview.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsxListedCompaniesController : ControllerBase
    {
        private IAsxListedCompaniesService _asxListedCompaniesService;

        public AsxListedCompaniesController(IAsxListedCompaniesService asxListedCompaniesService)
        {
            _asxListedCompaniesService = asxListedCompaniesService;
        }

        [HttpGet("{asxCode}")]
        public async Task<ActionResult<AsxListedCompany>> Get(string asxCode)
        {
            var asxListedCompany = await _asxListedCompaniesService.GetByAsxCode(asxCode);
            if (asxListedCompany == null)
                return NotFound();
            return Ok(asxListedCompany);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AsxListedCompany>>> Get()
        {
            var asxListedCompany = await _asxListedCompaniesService.GetAll();
            return Ok(asxListedCompany);
        }
    }
}
