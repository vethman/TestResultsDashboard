using System.Linq.Expressions;
using MongoDB.Entities;

namespace TestResultsDashboard.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    public async Task<TEntity?> FindOneByIdAsync(string id)
    {
        var entity = await DB.Find<TEntity>()
            .OneAsync(id);

        return entity;
    }

    public async Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<string> ids)
    {
        var entities = await DB.Find<TEntity>()
            .ManyAsync(x => ids.Contains(x.ID));

        return entities;
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity,bool>> expression)
    {
        var entities = await DB.Find<TEntity>()
            .ManyAsync(expression);

        return entities;
    }
    
    public async Task<TEntity?> FindOneAsync(Expression<Func<TEntity,bool>> expression)
    {
        var entities = await FindAsync(expression);

        return entities.SingleOrDefault();
    }

    public async Task<IEnumerable<TEntity>> FindAllAsync()
    {
        var entities = await DB.Find<TEntity>()
            .ManyAsync(_ => true);

        return entities;
    }

    public async Task<string> InsertAsync(TEntity entity)
    {
        await entity.SaveAsync();
        return entity.ID!;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        await DB.Update<TEntity>()
            .MatchID(entity.ID)
            .ModifyWith(entity)
            .ExecuteAsync();
    }

    public async Task DeleteAsync(string id)
    {
        await DB.DeleteAsync<TEntity>(id);
    }
}