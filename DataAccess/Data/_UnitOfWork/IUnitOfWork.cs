using DataAccess.Data.DbContext;
using DataAccess.Data.Repositories._GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data._UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public int Complete();
        public void Dispose();
    }
   
}
