public interface IRepository<T> : INotify
    where T : class
{
    Task<List<T>> GetAsync(CancellationToken token = default);

    Task<List<T>> GetAsync(int take, int? skip, CancellationToken token = default);

    IQueryable<T> GetIQueryable(CancellationToken token = default);

    Task<T> GetAsync(int id, CancellationToken token = default);

    Task<T> CreateAsync(T entity, CancellationToken token = default);

    T Update(T entity);

    bool Remove(int id);

    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default);

    Task<T> FindFirstAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default);

    Task<List<T>> FindRangeAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default);
}