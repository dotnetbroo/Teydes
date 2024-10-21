using System.Linq.Expressions;

namespace Teydes.Data.IRepositories;

public interface IRepository<TEnitity>
{
    public Task<bool> SaveAsync();
    public Task<bool> DeleteAsync(long id);
    public TEnitity Update(TEnitity enitity);
    public Task<TEnitity> InsertAsync(TEnitity enitity);
    public Task<TEnitity> SelectAsync(Expression<Func<TEnitity, bool>> expression, string[] includes = null);
    public IQueryable<TEnitity> SelectAll(Expression<Func<TEnitity, bool>> expression = null, string[] includes = null);
}
