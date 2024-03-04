using System.Linq.Expressions;

namespace HM.AppDatabase;

public interface IDatabase<T>
    where T : class
{
    T? Query(Expression<Func<T, bool>> predicate);

    IEnumerable<T> QueryMany(Expression<Func<T, bool>> predicate);

    void Add(T item);

    bool Update(Expression<Func<T, bool>> predicate, Action<T> valueUpdater);

    int UpdateMany(Expression<Func<T, bool>> predicate, Action<T> valueUpdater);

    void AddMany(IEnumerable<T> items);

    bool Delete(Expression<Func<T, bool>> predicate);

    int DeleteMany(Expression<Func<T, bool>> predicate);
}

public interface IAsyncDatabase<T>
    where T : class
{
    Task<T?> QueryAsync(Expression<Func<T, bool>> predicate);

    IAsyncEnumerable<T> QueryManyAsync(Expression<Func<T, bool>> predicate);

    Task AddAsync(T item);

    Task AddManyAsync(IEnumerable<T> items);

    Task<bool> UpdateAsync(Expression<Func<T, bool>> predicate, Action<T> valueUpdater);

    Task<int> UpdateManyAsync(Expression<Func<T, bool>> predicate, Action<T> valueUpdater);

    Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate);

    Task<int> DeleteManyAsync(Expression<Func<T, bool>> predicate);
}