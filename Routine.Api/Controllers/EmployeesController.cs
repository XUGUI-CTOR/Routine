using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.Entities;
using Routine.Api.Models;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly ICompanyRepository repository;
        private readonly IMapper mapper;

        public EmployeesController(ICompanyRepository repository, IMapper mapper)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesForCompany(Guid companyId, [FromQuery(Name = "gender")] string genderDisplay,string q)
        {
            if (!await repository.CompanyExistsAsync(companyId))
                return NotFound();
            var employees = await repository.GetEmployeesAsync(companyId, genderDisplay,q);
            var employeedtos = mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(employeedtos);
        }
        [HttpGet("{employeeId}",Name = nameof(GetEmployee))]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(Guid companyId, Guid employeeId)
        {
            if (!await repository.CompanyExistsAsync(companyId))
                return NotFound();
            var employee = await repository.GetEmployeeAsync(companyId, employeeId);
            if (employee == null)
                return NotFound();
            var employeedto = mapper.Map<EmployeeDto>(employee);
            return Ok(employeedto);
        }

        public async Task<ActionResult<EmployeeDto>> CreateEmployeeForCompany(Guid companyId, EmployeeAddDto employee)
        {
            if (!await repository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var entity = mapper.Map<EmployeeAddDto, Employee>(employee);
            repository.AddEmployee(companyId, entity);
            await repository.SaveAsync();
            return CreatedAtAction(nameof(GetEmployee), 
                new { companyId = companyId, employeeId = entity.Id }, mapper.Map<Employee, EmployeeDto>(entity));
        }
    }
}
