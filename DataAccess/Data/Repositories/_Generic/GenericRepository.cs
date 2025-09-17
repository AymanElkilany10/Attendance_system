using DataAccess.Data.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Data.Entities;

namespace DataAccess.Data.Repositories._GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly Attendance_SystemContext _context;

        public GenericRepository(Attendance_SystemContext context)
        {
           _context = context;
        }
        public async Task<IEnumerable<T>> GetAllASync()
        { 
            return await _context.Set<T>().Where(e => !e.IsDeleted).ToListAsync(); 
        }

        public IQueryable<T> GetAllAsIQueryable() => _context.Set<T>();
        
        public virtual async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            _context.Update(entity);
        }

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            _context.Update(entity);
        }

        
    }
}
