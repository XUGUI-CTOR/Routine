using Microsoft.EntityFrameworkCore;
using Routine.Api.Data;
using Routine.Api.DtoParameters;
using Routine.Api.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Services
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly RoutineDbContext context;
        public CompanyRepository(RoutineDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public void AddCompany(Company company)
        {
            company.Id = Guid.NewGuid();
            if (company.Employees != null)
            {
                foreach (var employee in company.Employees)
                {
                    employee.Id = Guid.NewGuid();
                }
            }
            context.Companies.Add(company);
        }

        public void AddEmployee(Guid companyId, Employee employee)
        {
            context.Companies.FirstOrDefault(x => x.Id == companyId)?.Employees?.Add(employee);
        }

        public Task<bool> CompanyExistsAsync(Guid companyId)
        {
            return Task.FromResult<bool>(context.Companies.Any(x => x.Id == companyId));
        }

        public void DeleteCompany(Company company)
        {
            context.Companies.Remove(company);
        }

        public void DeleteEmployee(Employee employee)
        {
            context.Employees.Remove(employee);
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(CompanyDtoParameters paras)
        {
            if (paras == null)
                throw new ArgumentNullException(nameof(paras));
            if (string.IsNullOrWhiteSpace(paras.CompanyName) && string.IsNullOrWhiteSpace(paras.SearchTerm))
                return await context.Companies.ToListAsync();
            var queryExpresstion = context.Companies as IQueryable<Company>;
            if (!string.IsNullOrWhiteSpace(paras.CompanyName))
            {
                paras.CompanyName = paras.CompanyName.Trim();
                queryExpresstion = queryExpresstion.Where(x => x.Name == paras.CompanyName);
            }
            if (!string.IsNullOrWhiteSpace(paras.SearchTerm))
            {
                paras.SearchTerm = paras.SearchTerm.Trim();
                queryExpresstion = queryExpresstion.Where(x => x.Name.Contains(paras.SearchTerm) || x.Introduction.Contains(paras.SearchTerm));
            }
            return await queryExpresstion.ToListAsync();
        }

        public Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyIds)
        {
            return Task.FromResult(context.Companies.Where(x => companyIds.Contains(x.Id)).OrderBy(x => x.Name).AsEnumerable());
        }

        public Task<Company> GetCompanyAsync(Guid companyId)
        {
            return Task.FromResult(context.Companies.FirstOrDefault(x => x.Id == companyId));
        }

        public Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId)
        {
            return Task.FromResult(context.Employees.FirstOrDefault(x => x.Id == employeeId && x.CompanyId == companyId));
        }

        public Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, string genderDisplay, string q)
        {
            if (string.IsNullOrWhiteSpace(genderDisplay) && string.IsNullOrWhiteSpace(q))
                return Task.FromResult(context.Employees.Where(x => x.CompanyId == companyId).OrderBy(x => x.EmployeeNo).AsEnumerable());
            var query = context.Employees.Where(x => x.CompanyId == companyId);
            if (!string.IsNullOrWhiteSpace(genderDisplay))
            {
                var gender = Enum.Parse<Gender>(genderDisplay.Trim());
                query = query.Where(x => x.Gender == gender);
            }
            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                query = query.Where(x => x.EmployeeNo.Contains(q) || x.FirstName.Contains(q) || x.LastName.Contains(q));
            }
            return Task.FromResult(query.OrderBy(x => x.EmployeeNo).AsEnumerable());
        }

        public Task<bool> SaveAsync()
        {
            return Task.FromResult(context.SaveChanges() > 0);
        }

        public void UpdateCompany(Company company)
        {
            //context.Companies.Update(company);
        }

        public void UpdateEmployee(Employee employee)
        {
            //context.Employees.Update(employee);
        }
    }
}
