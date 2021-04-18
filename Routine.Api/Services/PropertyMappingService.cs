using Routine.Api.Entities;
using Routine.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _employeePropertyMapping = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
            {"Id",new PropertyMappingValue(false,new List<string>{"Id" }) },
            {"CompanyId",new PropertyMappingValue(false,new List<string>(){"CompanyId" }) },
            {"EmployeeNo",new PropertyMappingValue(false,new List<string>{ "EmployeeNo"}) },
            {"Name",new PropertyMappingValue(false,new List<string>{"FirstName","LastName" }) },
            {"GenderDisplay",new PropertyMappingValue(false,new List<string>{ "Gender"}) },
            {"Age",new PropertyMappingValue(true,new List<string>{"DateOfBirth" }) }
        };
        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();
        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<EmployeeDto, Employee>(_employeePropertyMapping));
        }
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();
            if (matchingMapping.Count() == 1)
                return matchingMapping.First().MappingDictionnary;
            throw new Exception($"无法找到唯一的映射关系: {typeof(TSource)} {typeof(TDestination)}");
        }
    }
}
