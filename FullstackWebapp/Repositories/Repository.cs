using Microsoft.EntityFrameworkCore;
using FullstackWebapp.Data;

namespace FullstackWebapp.Repositories;

public class Repository<T>(AppDbContext context) : IRepository<T> where T : class
{
    protected readonly DbSet<T> _dbSet = context.Set<T>();
    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
    public async Task UpdateAsync(T entity) => _dbSet.Update(entity);
    public async Task SaveChangesAsync()
    => await context.SaveChangesAsync();
    public IQueryable<T> AsQueryable() => context.Set<T>().AsQueryable();
    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null) _dbSet.Remove(entity);
    }
}