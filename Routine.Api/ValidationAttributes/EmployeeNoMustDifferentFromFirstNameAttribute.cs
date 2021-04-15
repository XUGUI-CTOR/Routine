using Routine.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.ValidationAttributes
{
    public class EmployeeNoMustDifferentFromFirstNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var addDto = (EmployeeAddOrUpdateDto)validationContext.ObjectInstance;
            //(放在类上)等价于addDto = (EmployeeAddDto)value;
            if (addDto.FirstName == addDto.EmployeeNo)
                return new ValidationResult("员工编号不能与员工名重复",new[] {nameof(addDto.FirstName),nameof(addDto.EmployeeNo) });
            return ValidationResult.Success;
        }
    }
}
