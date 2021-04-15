using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Models
{
    public class CompanyAddDto
    {
        [Display(Name ="公司名称")]
        [Required(ErrorMessage = "{0}不能为null")]
        [MaxLength(100,ErrorMessage = "{0}的最大长度不能超过{1}")]
        public string Name { get; set; }
        [Display(Name="简介")]
        [StringLength(500, MinimumLength = 10,ErrorMessage = "{0}的长度为{2}到{1}")]
        public string Introduction { get; set; }
        public ICollection<EmployeeAddDto> Employees { get; set; }
    }
}
