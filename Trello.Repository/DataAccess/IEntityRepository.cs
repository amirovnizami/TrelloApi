using System.Linq.Expressions;
using Trello.Domain.Abstractions;
using Trello.Domain.Entities;

namespace Trello.Repository.DataAccess;

public interface IEntityRepository<T> where T : class,IEntity,new()
{
    T Get(Expression<Func<T, bool>> filter = null); 
    Task<List<T>> GetListAsync(Expression<Func<T, bool>> filter = null);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}