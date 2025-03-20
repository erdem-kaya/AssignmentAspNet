using Data.Contexts;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;

public class BaseRepository<TEntity>(DataContext context) : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly DataContext _context = context;
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    private IDbContextTransaction _transaction = null!;

    #region TransactionManagement
    public virtual async Task BeginTransactionAsync()
    {
        _transaction ??= await _context.Database.BeginTransactionAsync();
    }

    public virtual async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
        }
    }

    public virtual async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
        }
    }

    #endregion

    public virtual async Task<TEntity?> CreateAsync(TEntity entity)
    {
        if (entity == null)
            return entity;

        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating {nameof(TEntity)} entity : {ex.Message}");
            return null;
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? expression = null)
    {
        try
        {
            IQueryable<TEntity> query = _dbSet;
            if (expression != null)
                query = query.Where(expression);
            var result = await query.ToListAsync();
            return result;
        }
        catch (Exception ex) {
            Debug.WriteLine($"Error getting all {nameof(TEntity)} entities : {ex.Message}");
            return [];
        }
    }

    public virtual async Task<TEntity?> GetItemAsync(Expression<Func<TEntity, bool>> expression)
    {
        if (expression == null)
            return null;

        try
        {
            var result = await _dbSet.FirstOrDefaultAsync(expression);
            return result;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting {nameof(TEntity)} entity : {ex.Message}");
            return null;
        }
    }

    public virtual async Task<TEntity?> UpdateAsync(Expression<Func<TEntity, bool>> expression, TEntity updatedEntity)
    {
        if (expression == null || updatedEntity == null)
            return null;

        try
        {
            var entity = await _dbSet.FirstOrDefaultAsync(expression);
            if (entity == null)
                return null;

            _context.Entry(entity).CurrentValues.SetValues(updatedEntity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating {nameof(TEntity)} entity : {ex.Message}");
            return null;
        }
    }

    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression)
    {
        if (expression == null)
            return false;

        try
        {
            var entity = await _dbSet.FirstOrDefaultAsync(expression);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting {nameof(TEntity)} entity : {ex.Message}");
            return false;
        }
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        if (expression == null)
            return false;

        try
        {
            return await _dbSet.AnyAsync(expression);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error checking if {nameof(TEntity)} entity exists : {ex.Message}");
            return false;
        }
    }

}
