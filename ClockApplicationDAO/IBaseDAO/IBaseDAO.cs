using ClockApplicationBO.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClockApplicationDAO.IBaseDAO;

public class IBaseDAO<T> where T : class
{
    private readonly ClockApplicationContext _context;
    private readonly DbSet<T> _dbSet;

    public IBaseDAO()
    {
        _context = new ClockApplicationContext();
        _dbSet = _context.Set<T>();
    }

    public List<T> GetAllWithPaging(int pageNum, int pageSize)
    {
        return _dbSet.Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();
    }

    public List<T> GetAllWithInclude(params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet;

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return query.ToList();
    }

    public List<T> GetAll()
    {
        return _dbSet.ToList();
    }
    public void Create(T entity)
    {
        var tracker = _dbSet.Add(entity);
        _context.SaveChanges();
        tracker.State = EntityState.Detached;
    }
    public void Update(T entity)
    {
        var tracker = _context.Attach(entity);
        tracker.State = EntityState.Modified;
        _context.SaveChanges();
        tracker.State = EntityState.Detached;
    }
    public bool Remove(T entity)
    {
        _dbSet.Remove(entity);
        _context.SaveChanges();
        return true;
    }
    public T? GetById(int id)
    {
        return _dbSet.Find(id);
    }
}

