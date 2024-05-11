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
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity 
    {

        private protected readonly AppDbContext _context; //NULL


        public GenericRepository(AppDbContext context) // Ask CLR to create object from AppDbcontext
        {
            // _context = new AppDbContext();
            _context = context;
        }



        public void Add(T entity)
        {
             // _context.Set<T>().Add(entity);
            _context.AddAsync(entity);   
        }

        public void Delete(T entity)
        {
           //  _context.Set<T>().Remove(entity);
            _context.Remove(entity);
        }

        public async Task<T> Get(int id)
        {
            var entity = _context.Set<T>().FindAsync(id);
            return await entity;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            if (typeof(T) == typeof(Employee)) 
            {
                return (IEnumerable<T>) await  _context.Employees.Include(E=>E.Department).ToListAsync();
            }
            else
            {
                return await _context.Set<T>().ToListAsync();  
            }
           
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}
