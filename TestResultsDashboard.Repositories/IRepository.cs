using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Entities;

namespace TestResultsDashboard.Repositories;

public interface IRepository<TEntity> where TEntity : Entity
{
    Task<TEntity?> FindOneByIdAsync(string id);
    Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<string> ids);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity,bool>> expression);
    Task<IEnumerable<TEntity>> FindAllAsync();
    Task<string> InsertAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(string id);
    Task<TEntity?> FindOneAsync(Expression<Func<TEntity,bool>> expression);
}