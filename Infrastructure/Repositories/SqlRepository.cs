using UserOrdersApiDemo.Domain;
using UserOrdersApiDemo.Application;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace UserOrdersApiDemo.Infrastructure
{
    public class SqlRepository<T> : IRepository<T> where T : class, IEntity
    {
        protected DataContext _context;
        protected DbSet<T> dbSet;

        public SqlRepository(DataContext context)
        {
            this._context = context;
            this.dbSet = _context.Set<T>();
        }

        protected virtual IQueryable<T> BeforeGet()
        {
            var query = dbSet.AsQueryable();
            
            var navigations = this._context.Model.FindEntityType(typeof(T))
                .GetDerivedTypesInclusive()
                .SelectMany(type => type.GetNavigations())
                .Distinct();

            foreach (var property in navigations)
                query = query.Include(property.Name);

            return query;
        }

        public IEnumerable<T> Get() => BeforeGet()
            .AsNoTracking()
            .ToList();

        public T Find(int id) => BeforeGet()
            .AsNoTracking()
            .OrderBy(entity => entity.Id)
            .FirstOrDefault(entity => entity.Id == id);

        public void Insert(T entity) => dbSet.Add(entity);

        public void Update(T entity) => _context.Entry(entity)
            .State = EntityState.Modified;

        public void Delete(int id) => dbSet.Remove(dbSet.Find(id));

        public void Save() => _context.SaveChanges();

        public bool Exists(int id) => dbSet.Any(entry => entry.Id == id);
    }
}