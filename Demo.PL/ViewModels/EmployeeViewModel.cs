using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using Microsoft.AspNetCore.Http;
using Demo.DAL.Models;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required!!")]
        public string Name { get; set; }
        [Range(22, 45)]
        public int? Age { get; set; }
        public decimal Salary { get; set; }
        [RegularExpression(@"[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
                          ErrorMessage = "Address must be like 123-street-city-country")]
        public string Address { get; set; }
        [Phone]
        [Required]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [DisplayName("Hiring Date")]
        [Required(ErrorMessage = "Hiring Date Is Required")]
        public DateTime HireDate { get; set; }
        public string ImageName { get; set; }
        [Required(ErrorMessage = "Image Is Required")]

        public IFormFile Image { get; set; }
        [Required(ErrorMessage = "Department Is Required")]

        public int? DepartmentId { get; set; }//FK
        public Department Department { get; set; }

    }
}
