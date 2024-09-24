using Microsoft.EntityFrameworkCore;

namespace Schemio.EF
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}