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
using Routine.Api.Helpers;
using Newtonsoft.Json;

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
        [HttpGet(Name = nameof(GetCompanies))]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies([FromQuery] CompanyDtoParameters paras)
        {
            var companies = await services.GetCompaniesAsync(paras);
            var previousLink = companies.HasPrevious ? CreateCompaniesResourceUri(paras, ResourceUriType.PreviousPage) : string.Empty;
            var nextLink = companies.HasNext ? CreateCompaniesResourceUri(paras, ResourceUriType.NextPage) : string.Empty;
            var paginationMetadata = new
            {
                companies.PageSize,
                companies.TotalCount,
                companies.TotalPages,
                companies.CurrentPage,
                PreviousLink = previousLink,
                NextLink = nextLink
            };
            Response.Headers.Add("X-Paginatiop", JsonConvert.SerializeObject(paginationMetadata));
            var companydtos = mapper.Map<IEnumerable<CompanyDto>>(companies);
            return Ok(companydtos);
        }

        private string CreateCompaniesResourceUri(CompanyDtoParameters parameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        PageNumber = parameters.PageNumber - 1,
                        parameters.PageSize,
                        parameters.CompanyName,
                        parameters.SearchTerm
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        PageNumber = parameters.PageNumber + 1,
                        PageSize = parameters.PageSize,
                        parameters.CompanyName,
                        parameters.SearchTerm
                    });
                default:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        PageNumber = parameters.PageNumber,
                        PageSize = parameters.PageSize,
                        parameters.CompanyName,
                        parameters.SearchTerm
                    });
            }
        }

        [HttpGet("{companyid}", Name = nameof(GetCompanyById))]
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

        [HttpDelete("{companyid}")]
        public async Task<IActionResult> DeleteCompany(Guid companyid)
        {
            var entity = await services.GetCompanyAsync(companyid);
            if (entity == null)
                return NotFound();
            services.DeleteCompany(entity);
            await services.SaveAsync();
            return NoContent();
        }
    }
}
