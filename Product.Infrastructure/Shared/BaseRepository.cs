using System;
using System.Linq;
using Template.Shared;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Product.Infrastructure.Shared
{
    public class BaseRepository<TContext, TAggregateRoot> : IRepository<TAggregateRoot>
       where TAggregateRoot : AggregateRoot
       where TContext : DbContext
    {
        TContext _context;

        public BaseRepository(TContext context)
        {
            _context = context;
        }

        public void Delete(TAggregateRoot aggregateRoot)
        {
            _context.Set<TAggregateRoot>().Remove(aggregateRoot);
        }

        public async Task<TAggregateRoot> GetByIdAsync(int id)
        {
            return await _context.Set<TAggregateRoot>().FindAsync(id);
        }

        private void Insert(TAggregateRoot aggregateRoot)
        {
            _context.Set<TAggregateRoot>().Add(aggregateRoot);
        }

        private void Update(TAggregateRoot aggregateRoot)
        {
            _context.Entry(aggregateRoot).State = EntityState.Modified;
        }

        public IQueryable<TAggregateRoot> Query(Expression<Func<TAggregateRoot, bool>> expression = null)
        {
            return expression == null ? _context.Set<TAggregateRoot>().AsQueryable() : _context.Set<TAggregateRoot>().Where(expression);
        }

        public void Save(TAggregateRoot aggregateRoot)
        {
            if (aggregateRoot.Id <= 0)
            {
                Insert(aggregateRoot);
            }
            else
            {
                Update(aggregateRoot);
            }
        }
    }
}
