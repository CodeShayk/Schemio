using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Schemio.Object.EF
{
    public interface IDbContext
    {
        //DbSet<TEntity> Set<[DynamicallyAccessedMembers(IEntityType.DynamicallyAccessedMemberTypes)] TEntity>()
        //where TEntity : class;
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }

    internal class SchemioContext : DbContext, IDataContext
    {
        public string[] Paths { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public decimal CurrentVersion => throw new NotImplementedException();
    }
}