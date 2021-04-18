using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.DtoParameters;
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
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeDtoParameters paras)
        {
            if (!await repository.CompanyExistsAsync(companyId))
                return NotFound();
            var employees = await repository.GetEmployeesAsync(companyId, paras);
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
        [HttpPost]
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

        [HttpPut("{employeeId}")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployeeForCompany(Guid companyId, Guid employeeId, EmployeeUpdateDto dto)
        {
            if (!await repository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var employeeEntity = await repository.GetEmployeeAsync(companyId, employeeId);
            if (employeeEntity == null)
            {
                var AddEntity = mapper.Map<Employee>(dto);
                //AddEntity.CompanyId = companyId;
                //AddEntity.Id = employeeId;
                repository.AddEmployee(companyId, AddEntity);
                await repository.SaveAsync();
                return CreatedAtAction(nameof(GetEmployee),
               new { companyId = companyId, employeeId = AddEntity.Id }, mapper.Map<Employee, EmployeeDto>(AddEntity));
            }
            employeeEntity = mapper.Map(dto, employeeEntity);
            repository.UpdateEmployee(employeeEntity);
            await repository.SaveAsync();
            return NoContent();  
        }

        [HttpPatch("{employeeId}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid companyId, Guid employeeId, JsonPatchDocument<EmployeeUpdateDto> document)
        {
            if (!await repository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var employeeEntity = await repository.GetEmployeeAsync(companyId, employeeId);
            if (employeeEntity == null)
            {
                var employeeUpdateDto = new EmployeeUpdateDto();
                document.ApplyTo(employeeUpdateDto, ModelState);//如果处理发生错误，将错误信息写入ModelStateMap中
                if (!TryValidateModel(employeeUpdateDto))//Pacth传入时不会调用model验证，所以需要手动进行验证
                {
                    return ValidationProblem(ModelState);
                    //return UnprocessableEntity(ModelState);
                }
                var AddEntity = mapper.Map<EmployeeUpdateDto, Employee>(employeeUpdateDto);
                repository.AddEmployee(companyId, AddEntity);
                await repository.SaveAsync();
                return CreatedAtAction(nameof(GetEmployee),
               new { companyId = companyId, employeeId = AddEntity.Id }, mapper.Map<Employee, EmployeeDto>(AddEntity));
            }

            var dtoToPatch = mapper.Map<EmployeeUpdateDto>(employeeEntity);
            document.ApplyTo(dtoToPatch, ModelState);//如果处理发生错误，将错误信息写入ModelStateMap中
            if (!TryValidateModel(dtoToPatch))//Pacth传入时不会调用model验证，所以需要手动进行验证
            {
                return ValidationProblem(ModelState);
                //return UnprocessableEntity(ModelState);
            }
            mapper.Map<EmployeeUpdateDto, Employee>(dtoToPatch, employeeEntity);
            repository.UpdateEmployee(employeeEntity);
            await repository.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid employeeId)
        {

            if (!await repository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var employeeEntity = await repository.GetEmployeeAsync(companyId, employeeId);
            if (employeeEntity == null)
            {
                return NotFound();
            }
            repository.DeleteEmployee(employeeEntity);
            await repository.SaveAsync();
            return NoContent();
        }
    }
}
