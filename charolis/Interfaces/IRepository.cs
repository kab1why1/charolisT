using System.Collections.Generic;

namespace charolis.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        void Add(T entity);
        void Remove(T entity);
        T? GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetSorted();
    }
}
