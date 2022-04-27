using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetById(int id);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<T> AddAsync(T entity);
        T Update(T entity);
        void Delete(T entity);
    }
}
