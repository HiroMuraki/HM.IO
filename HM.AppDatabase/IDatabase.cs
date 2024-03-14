using System.Linq.Expressions;

namespace HM.AppDatabase;

public interface IDatabase<T>
    where T : class
{
    T? Query(Expression<Func<T, Boolean>> predicate);

    IEnumerable<T> QueryMany(Expression<Func<T, Boolean>> predicate);

    void Add(T item);

    Boolean Update(Expression<Func<T, Boolean>> predicate, Action<T> valueUpdater);

    Int32 UpdateMany(Expression<Func<T, Boolean>> predicate, Action<T> valueUpdater);

    void AddMany(IEnumerable<T> items);

    Boolean Delete(Expression<Func<T, Boolean>> predicate);

    Int32 DeleteMany(Expression<Func<T, Boolean>> predicate);
}

public interface IAsyncDatabase<T>
    where T : class
{
    Task<T?> QueryAsync(Expression<Func<T, Boolean>> predicate);

    IAsyncEnumerable<T> QueryManyAsync(Expression<Func<T, Boolean>> predicate);

    Task AddAsync(T item);

    Task AddManyAsync(IEnumerable<T> items);

    Task<Boolean> UpdateAsync(Expression<Func<T, Boolean>> predicate, Action<T> valueUpdater);

    Task<Int32> UpdateManyAsync(Expression<Func<T, Boolean>> predicate, Action<T> valueUpdater);

    Task<Boolean> DeleteAsync(Expression<Func<T, Boolean>> predicate);

    Task<Int32> DeleteManyAsync(Expression<Func<T, Boolean>> predicate);
}