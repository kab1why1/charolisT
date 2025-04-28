using charolis.Entity;
using charolis.Storage;

namespace charolis.Data;

public class GenericRepository<T> where T : BaseEntity
{
    private readonly IDataStorage<T> _storage;

    public GenericRepository(IDataStorage<T> storage)
    {
        _storage = storage;
    }

    public List<T> GetAll() => _storage.GetAll();

    public T? GetById(int id) => _storage.GetById(id);

    public void Add(T entity)
    {
        _storage.Add(entity);
        _storage.Save();
    }

    public void Update(T entity)
    {
        _storage.Update(entity);
        _storage.Save(); 
    }

    public void Delete(int id)
    {
        _storage.Delete(id);
        _storage.Save(); 
    }
}