using System.Collections.Generic;

namespace UserOrdersApiDemo.Domain
{
    public interface IRepository<T> where T : class, IEntity
    {
        IEnumerable<T> Get();
        T Find(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(int id);
        bool Exists(int id);
        void Save();
    }
}