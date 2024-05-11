using System;
using System.Collections.Generic;
namespace Demo.DAL.Models
{

    // Model
    public class Department : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
