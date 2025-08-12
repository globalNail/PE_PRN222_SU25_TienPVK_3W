using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repositories.Implements
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly Su25pantherDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(Su25pantherDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().AnyAsync(predicate);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate == null)
            {
                return await _dbSet.AsNoTracking().CountAsync();
            }
            return await _dbSet.CountAsync(predicate);
        }

        public virtual async Task<int> DeleteAsync(T entity)
        {
            var entry = _context.ChangeTracker.Entries<T>().FirstOrDefault(e => e.Entity == entity);
            if (entry == null)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>? include = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>? include = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if (include != null)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public virtual async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>? include = null)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if (include != null)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<int> UpdateAsync(T entity)
        {
            var entry = _context.ChangeTracker.Entries<T>().FirstOrDefault(e => e.Entity == entity);
            if (entry != null)
            {
                entry.State = EntityState.Modified;
            }
            else
            {
                _dbSet.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<(IEnumerable<T> items, int totalCount, int totalPages, bool hasPrevious, bool hasNext)> GetPagedAsync(int pageIndex, int pageSize, Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>? include = null, Expression<Func<T, object>>? orderBy = null, bool orderByDescending = false)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (include != null)
            {
                query = query.Include(include);
            }

            if (orderBy != null)
            {
                if (orderByDescending)
                {
                    query = query.OrderByDescending(orderBy);
                }
                else
                {
                    query = query.OrderBy(orderBy);
                }
            }

            int totalcount = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalcount / pageSize);
            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            bool hasPrevious = pageIndex > 1;
            bool hasNext = (pageIndex * pageSize) < totalcount;
            return (items, totalcount, totalPages, hasPrevious, hasNext);
        }
    }
}
