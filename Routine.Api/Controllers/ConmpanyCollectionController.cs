using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Entities;
using Routine.Api.Helpers;
using Routine.Api.Models;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Controllers
{
    [ApiController,Route("api/[controller]")]
    public class CompanyCollectionController:ControllerBase
    {
        private readonly ICompanyRepository service;
        private readonly IMapper mapper;

        public CompanyCollectionController(ICompanyRepository service,IMapper mapper)
        {
            this.service = service??throw new ArgumentNullException(nameof(service));
            this.mapper = mapper?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet("{ids}", Name =nameof(GetCompanyCollection))]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanyCollection([FromRoute, ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
                return BadRequest();
            var entities = await service.GetCompaniesAsync(ids);
            if (entities == null || entities.Count() == 0)
                return NotFound();
            var dtoReturns = mapper.Map<IEnumerable<CompanyDto>>(entities);
            return Ok(dtoReturns);
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> CreateCompanys(IEnumerable<CompanyAddDto> companies)
        {
            if (companies.Count() == 0)
                return NotFound();
            var entities = mapper.Map<IEnumerable<Company>>(companies);
            foreach (var item in entities)
            {
                service.AddCompany(item);
            }
            await service.SaveAsync();
            List<CompanyDto> dtoToReturns = mapper.Map<List<CompanyDto>>(entities);
            var ids = entities.Select(x => x.Id.ToString()).Aggregate((x,y)=>x+=(","+y));
            return CreatedAtAction(nameof(GetCompanyCollection), new { ids = ids }, dtoToReturns);
        }
    }
}
