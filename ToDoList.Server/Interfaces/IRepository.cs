using System.Linq.Expressions;

namespace ToDoList.Interfaces;
public interface IRepository<T> : INotify
    where T : class
{
    Task<List<T>> GetAsync(CancellationToken token = default);

    Task<T> GetAsync(int id, CancellationToken token = default);

    Task<T> CreateAsync(T entity, CancellationToken token = default);

    Task<T> UpdateAsync(T entity, CancellationToken token = default);

    Task<bool> RemoveAsync(int id, CancellationToken token = default);
}