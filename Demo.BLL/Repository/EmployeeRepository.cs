using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repository
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository( AppDbContext context ) : base ( context ) // ASK CLR To Create Object From AppDbContext
        {
            
        }

        public IEnumerable<Employee> GetByName(string Name)
        {
           return _context.Employees.Where(E=>E.Name.ToLower().Contains(Name.ToLower())).Include(E=>E.Department).ToList();    
        }

     
    }
}
