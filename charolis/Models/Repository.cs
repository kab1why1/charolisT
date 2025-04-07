using System;
using charolis.Interfaces;

namespace charolis.Models;

public class Repository<T> : IRepository<T> where T : IEntity
{
    protected static List<T> _entities = new List<T>(); // Статичний список зберігає дані між запитами

    public  void Add(T entity)
    {
        if (!_entities.Any(e => e.Id == entity.Id)) // Перевіряємо унікальність ID
        {
            _entities.Add(entity);
        }
    }

    public void Remove(T entity) => _entities.Remove(entity);
    public T GetById(int id) => _entities.FirstOrDefault(e => e.Id == id);
    public IEnumerable<T> GetAll() => _entities.ToList(); // Повертаємо копію списку
    public IEnumerable<T> GetSorted() => _entities.OrderBy(e => e.Id).ToList(); // Сортуємо список
}
