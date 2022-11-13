using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Restaurant.Messages.InMemoryDb;

public class InMemoryRepository<T> : IInMemoryRepository<T> where T : class
{
    private readonly ConcurrentBag<T> _repo = new(); //создаем коллекцию для хранения днных

    /// <summary>
    /// Добавляем операция
    /// </summary>
    /// <param name="entity"></param>
    public void AddOrUpdate(T entity)
    {
        _repo.Add(entity);
    }

    /// <summary>
    /// Извлекаем операцию
    /// </summary>
    /// <returns></returns>
    public IEnumerable<T> Get()
    {
        return _repo;
    }
}