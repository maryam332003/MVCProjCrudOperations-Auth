using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.DAL.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Demo.BLL.Repository
{
    public class DepartmentRepository :  GenericRepository<Department> , IDepartmentRepository
    {
        public DepartmentRepository( AppDbContext  context) : base(context) // ASK CLR To Create Object From AppDbContext
        {
        }

        //public IEnumerable<Department> GetByName(string name)
        //{
        //   return _context.Department.Where(D=>D.Name.ToLower().Contains(name.ToLower())).ToList();
        //}

        public IEnumerable<Department> GetByName(string Name)
        {
            return _context.Department.Where(E => E.Name.ToLower().Contains(Name.ToLower())).ToList();
            //return _context.Employees.Where(E => E.Name.ToLower().Contains(Name.ToLower())).Include(E => E.Department).ToList();

        }

    }
}
