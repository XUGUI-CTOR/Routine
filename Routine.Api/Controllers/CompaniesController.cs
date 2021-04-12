using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Routine.Api.DtoParameters;
using Routine.Api.Models;
using Routine.Api.Services;
using Routine.Api.Entities;
namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository services;
        private readonly IMapper mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="services">初始化服务类</param>
        public CompaniesController(ICompanyRepository services, IMapper mapper)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies([FromQuery]CompanyDtoParameters paras)
        {
            var companies = await services.GetCompaniesAsync(paras);
            var companydtos = mapper.Map<IEnumerable<CompanyDto>>(companies);
            return Ok(companydtos);
        }
        [HttpGet("{companyid}",Name =nameof(GetCompanyById))]
        public async Task<ActionResult<CompanyDto>> GetCompanyById(Guid companyid)
        {
            //var exist = await services.CompanyExistsAsync(companyid);
            //if (!exist)
            //    return NotFound();
            var company = await services.GetCompanyAsync(companyid);
            if (company == default)
                return NotFound();
            return Ok(mapper.Map<CompanyDto>(company));
        }
        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany(CompanyAddDto company)
        {
            var entity = mapper.Map<CompanyAddDto, Company>(company);
            services.AddCompany(entity);
            await services.SaveAsync();
            return CreatedAtAction(nameof(GetCompanyById), new { companyid = entity.Id }, mapper.Map<Company, CompanyDto>(entity));
        }
        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,OPTIONS");
            return Ok();
        }
    }
}
