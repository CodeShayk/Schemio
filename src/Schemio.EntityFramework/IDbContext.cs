using Microsoft.EntityFrameworkCore;

namespace Schemio.EntityFramework
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}