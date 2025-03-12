using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trello.Domain.Abstractions;
using Trello.Domain.Entities;

namespace Trello.Repository.DataAccess.EntityFrameworkAccess;

public class EfEntityRepositoryBase
{
    public class EFEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new() where TContext : DbContext, new()
    {
        public TEntity Get(Expression<Func<TEntity, bool>> filter = null)
        {
            using var context = new TContext();
            return context.Set<TEntity>().SingleOrDefault(filter);
        }
        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            using var context = new TContext();
            return filter == null
                ? await context.Set<TEntity>().ToListAsync()
                : await context.Set<TEntity>().Where(filter).ToListAsync();
        }

        public void Add(TEntity entity)
        {
            using var context = new TContext();
            // var addedEntity = context.Entry(entity);
            // addedEntity.State = EntityState.Added;
            context.Add(entity);
            context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            using var context = new TContext();
            var addedEntity = context.Entry(entity);
            addedEntity.State = EntityState.Modified;
            // context.Update(addedEntity);
            context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            using var context = new TContext();
            var addedEntity = context.Entry(entity);
            addedEntity.State = EntityState.Deleted;
            // context.Remove(addedEntity);
            context.SaveChanges();
        }
    }
}