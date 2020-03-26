
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace SwaggerAspCoreOData.Infastructure
{

  /// <summary>
  /// Provides generic operations to manage entities
  /// </summary>
  /// <typeparam name="TEntity"></typeparam>
  public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
  {
    private readonly DbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    /// <summary>
    /// Initialises the controller
    /// </summary>
    /// <param name="dbContext"></param>
    public Repository(DbContext dbContext)
    {
      _dbContext = dbContext;
      _dbSet = _dbContext.Set<TEntity>();
    }

    /// <summary>
    /// Adds an entity
    /// </summary>
    /// <param name="entity"></param>
    void IRepository<TEntity>.Add(TEntity entity)
    {
      _dbSet.Add(entity);
    }

    /// <summary>
    /// Add a range of entities
    /// </summary>
    /// <param name="entities"></param>
    void IRepository<TEntity>.AddRange(ICollection<TEntity> entities)
    {
      _dbSet.AddRange(entities);
    }

    /// <summary>
    /// Calls the save changes method at the database level
    /// </summary>
    void IRepository<TEntity>.Save()
    {
      _dbContext.SaveChanges();
    }

    /// <summary>
    /// Deletes an entity
    /// </summary>
    /// <param name="entity"></param>
    void IRepository<TEntity>.Delete(TEntity entity)
    {
      _dbSet.Remove(entity);
    }

    /// <summary>
    /// Deletes a range of entities
    /// </summary>
    /// <param name="entities"></param>
    void IRepository<TEntity>.DeleteRange(ICollection<TEntity> entities)
    {
      _dbSet.RemoveRange(entities);
    }

    /// <summary>
    /// Finds entities based on a predicate
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    IEnumerable<TEntity> IRepository<TEntity>.Find(Expression<Func<TEntity, bool>> predicate)
    {
      return _dbSet.Where(predicate);
    }

    /// <summary>
    /// Gets all entities
    /// </summary>
    /// <returns></returns>
    IEnumerable<TEntity> IRepository<TEntity>.GetAll()
    {
      return _dbSet.Select(x => x);
    }

    /// <summary>
    /// Gets an entity by key
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    TEntity IRepository<TEntity>.GetByKey(object Id)
    {
      return _dbSet.Find(Id);
    }

    /// <summary>
    /// Updates an entity
    /// </summary>
    /// <param name="entity"></param>
    void IRepository<TEntity>.Update(TEntity entity)
    {
      _dbSet.Update(entity);
    }
  }
}

