using charolis.Entity;

namespace charolis.Storage;

public interface IDataStorage<T> where T : BaseEntity
{
    List<T> GetAll();
    T? GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
    void Save();
}