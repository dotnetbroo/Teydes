using Teydes.Domain.Commons;
using Teydes.Data.DbContexts;
using System.Linq.Expressions;
using Teydes.Data.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Teydes.Data.Repositories;

public class Repository<TEnitity> : IRepository<TEnitity> where TEnitity : Auditable
{
    AppDbContext dbContext;
    DbSet<TEnitity> dbSet;
    public Repository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
        this.dbSet = dbContext.Set<TEnitity>();
    }
    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await this.dbSet.FirstOrDefaultAsync(e => e.Id == id);
        dbSet.Remove(entity);
        return true;

    }
    public async Task<TEnitity> InsertAsync(TEnitity enitity)
        =>  (await this.dbSet.AddAsync(enitity)).Entity;

    public async Task<bool> SaveAsync()
        => (await this.dbContext.SaveChangesAsync() > 0);

    public IQueryable<TEnitity> SelectAll(Expression<Func<TEnitity, bool>> expression = null, string[] includes = null)
    {
        var query = expression is null? dbSet : dbSet.Where(expression);
        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query;
    }

    public async Task<TEnitity> SelectAsync(Expression<Func<TEnitity, bool>> expression, string[] includes = null)
        => await this.SelectAll(expression, includes).FirstOrDefaultAsync();
    
    public TEnitity Update(TEnitity enitity)
        => this.dbSet.Update(enitity).Entity;
   
}
