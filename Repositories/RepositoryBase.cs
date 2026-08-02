using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore;

namespace Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T>
    where T : class
{
    protected readonly RepositoryContext _context;

    public RepositoryBase(RepositoryContext context)
    {
        _context = context;
    }

    public void Create(T entity) => _context.Set<T>().Add(entity);

    public void Delete(T entity) => _context.Set<T>().Remove(entity);

    // bunlar virtual ile yazılabilir. üstüne ekleme yapılmayacak sınıfta (orderby falan) hiç tekrardan yazılmasına gerek kalmaz.
    public IQueryable<T> FindAll(bool trackChanges) =>
        trackChanges ? _context.Set<T>() : _context.Set<T>().AsNoTracking();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
        trackChanges
            ? _context.Set<T>().Where(expression)
            : _context.Set<T>().Where(expression).AsNoTracking();

    public void Update(T entity) => _context.Set<T>().Update(entity);
}
