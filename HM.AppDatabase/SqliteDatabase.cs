using HM.AppDatabase;
using HM.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppDatabase;

public sealed partial class SqliteDatabase<T> :
    IDatabase<T>,
    IAsyncDatabase<T>,
    IDisposable
    where T : class
{
    public static SqliteDatabase<T> Create(String dbFilePath, String tableName)
    {
        var dbContext = SqliteContext.Create(dbFilePath, tableName);
        var sqliteDataBase = new SqliteDatabase<T>(dbContext);
        return sqliteDataBase;
    }

    public async Task InitializeAsync()
    {
        if (_isInitialized)
        {
            return;
        }

        await _dbContext.Database.EnsureCreatedAsync();
        _isInitialized = true;
    }

    public async Task SaveChangesAsync()
    {
        CheckStates();

        await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    ~SqliteDatabase()
    {
        Dispose(disposing: false);
    }

    #region NonPublic
    private SqliteDatabase(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    private Boolean _isInitialized;
    private Boolean _isDisposed;
    private ReaderWriterLockSlim _readerWriterLockSlim = new();
    private DbContext _dbContext;
    private T? QueryHelper(Expression<Func<T, Boolean>> predicate, Boolean asNoTracking)
    {
        return QueryManyHelper(predicate, asNoTracking).SingleOrDefault();
    }
    private async Task<T?> QueryHelperAsync(Expression<Func<T, Boolean>> predicate, Boolean asNoTracking)
    {
        return await QueryManyHelper(predicate, asNoTracking).SingleOrDefaultAsync();
    }
    private IQueryable<T> QueryManyHelper(Expression<Func<T, Boolean>> predicate, Boolean asNoTracking)
    {
        CheckStates();

        IQueryable<T> set;

        if (typeof(T).IsAssignableTo(typeof(IDbEntity)))
        {
            set = _dbContext.Set<T>()
                .Cast<IDbEntity>()
                .Where(x => !x.IsDeleted)
                .Cast<T>()
                .Where(predicate);
        }
        else
        {
            set = _dbContext.Set<T>()
                .Where(predicate);
        }

        if (asNoTracking)
        {
            set = set.AsNoTracking();
        }

        return set;
    }
    private void AddHelper(T item)
    {
        if (item is IDbEntity dbEntity)
        {
            Int64 createTime = DateTime.Now.Ticks;
            dbEntity.CreateTime = createTime;
            dbEntity.UpdateTime = createTime;
        }

        _dbContext.Set<T>().Add(item);
    }
    private async Task AddHelperAsync(T item)
    {
        if (item is IDbEntity dbEntity)
        {
            Int64 createTime = DateTime.Now.Ticks;
            dbEntity.CreateTime = createTime;
            dbEntity.UpdateTime = createTime;
        }

        await _dbContext.Set<T>().AddAsync(item);
    }
    private void UpdateHelper(T item, Action<T> valueUpdater)
    {
        valueUpdater(item);

        if (item is IDbEntity dbEntity)
        {
            dbEntity.UpdateTime = DateTime.Now.Ticks;
        }
    }
    private void DeleteHelper(T item)
    {
        if (item is DbEntity dbEntity)
        {
            dbEntity.IsDeleted = true;
        }
        else
        {
            _dbContext.Set<T>().Remove(item);
        }
    }
    private void CheckStates()
    {
        if (!_isInitialized)
        {
            throw new InvalidOperationException("Database was not initialized");
        }

        if (_isDisposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }
    private void Dispose(Boolean disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            _readerWriterLockSlim.Dispose();
            _readerWriterLockSlim = null!;
            _dbContext.Dispose();
            _dbContext = null!;
        }

        _isDisposed = true;
    }
    private class SqliteContext : DbContext
    {
        public static SqliteContext Create(String sqliteFilePath, String tableName)
        {
            return new SqliteContext(tableName, sqliteFilePath);
        }

        public override void Dispose()
        {
            base.Dispose();

            _conn.Dispose();
        }

        #region NonPublic
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_conn)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<T>().ToTable(_tableName);
        }
        private readonly String _tableName;
        private readonly SqliteConnection _conn;
        private SqliteContext(String tableName, String sqliteFilePath)
        {
            _tableName = tableName;
            _conn = new SqliteConnection($"Data Source=\"{sqliteFilePath}\";Pooling=False;");
        }
        #endregion
    }
    #endregion
};

/// Partial of Implying the <see cref="IDatabase{T}{T}"/>
public sealed partial class SqliteDatabase<T>
{
    public T? Query(Expression<Func<T, Boolean>> predicate)
    {
        return ParallelSafe.Run(() =>
        {
            CheckStates();

            return QueryHelper(predicate, asNoTracking: true);
        }, _readerWriterLockSlim.EnterReadLock, _readerWriterLockSlim.ExitReadLock);
    }

    public IEnumerable<T> QueryMany(Expression<Func<T, Boolean>> predicate)
    {
        return ParallelSafe.Run(() =>
        {
            CheckStates();

            return QueryManyHelper(predicate, asNoTracking: true);
        }, _readerWriterLockSlim.EnterReadLock, _readerWriterLockSlim.ExitReadLock);
    }

    public void Add(T item)
    {
        ParallelSafe.Run(() =>
        {
            CheckStates();

            AddHelper(item);
        }, _readerWriterLockSlim.EnterWriteLock, _readerWriterLockSlim.ExitWriteLock);
    }

    public void AddMany(IEnumerable<T> items)
    {
        ParallelSafe.Run(() =>
        {
            CheckStates();

            foreach (T item in items)
            {
                AddHelper(item);
            }
        }, _readerWriterLockSlim.EnterWriteLock, _readerWriterLockSlim.ExitWriteLock);
    }

    public Boolean Update(Expression<Func<T, Boolean>> predicate, Action<T> valueUpdater)
    {
        return ParallelSafe.Run(() =>
        {
            CheckStates();

            T? targetItem = QueryHelper(predicate, asNoTracking: false);
            if (targetItem is null)
            {
                return false;
            }

            UpdateHelper(targetItem, valueUpdater);

            return true;
        }, _readerWriterLockSlim.EnterWriteLock, _readerWriterLockSlim.ExitWriteLock);
    }

    public Int32 UpdateMany(Expression<Func<T, Boolean>> predicate, Action<T> valueUpdater)
    {
        return ParallelSafe.Run(() =>
        {
            CheckStates();

            IQueryable<T> targetItems = QueryManyHelper(predicate, asNoTracking: false);
            Int32 count = 0;

            foreach (T targetItem in targetItems)
            {
                UpdateHelper(targetItem, valueUpdater);
                count++;
            }

            return count;
        }, _readerWriterLockSlim.EnterWriteLock, _readerWriterLockSlim.ExitWriteLock);
    }

    public Boolean Delete(Expression<Func<T, Boolean>> predicate)
    {
        return ParallelSafe.Run(() =>
        {
            CheckStates();

            T? targetItem = QueryHelper(predicate, asNoTracking: false);
            if (targetItem is null)
            {
                return false;
            }

            DeleteHelper(targetItem);

            return true;
        }, _readerWriterLockSlim.EnterWriteLock, _readerWriterLockSlim.ExitWriteLock);
    }

    public Int32 DeleteMany(Expression<Func<T, Boolean>> predicate)
    {
        return ParallelSafe.Run(() =>
        {
            CheckStates();

            DbSet<T> dbSet = _dbContext.Set<T>();
            IQueryable<T> targetItems = QueryManyHelper(predicate, asNoTracking: false);
            Int32 count = 0;

            foreach (T item in targetItems)
            {
                DeleteHelper(item);
                count++;
            }

            return count;
        }, _readerWriterLockSlim.EnterWriteLock, _readerWriterLockSlim.ExitWriteLock);
    }
}

/// Partial of Implying the <see cref="IAsyncDatabase{T}"/>
public sealed partial class SqliteDatabase<T>
{
    public async Task<T?> QueryAsync(Expression<Func<T, Boolean>> predicate)
    {
        return await ParallelSafe.RunAsync(async () =>
        {
            CheckStates();

            return await QueryHelperAsync(predicate, asNoTracking: true);
        }, _readerWriterLockSlim.EnterReadLock, _readerWriterLockSlim.ExitReadLock);
    }

    public async IAsyncEnumerable<T> QueryManyAsync(Expression<Func<T, Boolean>> predicate)
    {
        foreach (T item in QueryMany(predicate))
        {
            yield return item;
        }

        await Task.CompletedTask;
    }

    public async Task AddAsync(T item)
    {
        await ParallelSafe.RunAsync(async () =>
        {
            CheckStates();

            await AddHelperAsync(item);
        }, _readerWriterLockSlim.EnterWriteLock, _readerWriterLockSlim.ExitWriteLock);
    }

    public async Task AddManyAsync(IEnumerable<T> items)
    {
        await ParallelSafe.RunAsync(async () =>
        {
            CheckStates();

            foreach (T item in items)
            {
                await AddHelperAsync(item);
            }
        }, _readerWriterLockSlim.EnterWriteLock, _readerWriterLockSlim.ExitWriteLock);
    }

    public async Task<Boolean> UpdateAsync(Expression<Func<T, Boolean>> predicate, Action<T> valueUpdater)
    {
        return await Task.FromResult(Update(predicate, valueUpdater));
    }

    public async Task<Int32> UpdateManyAsync(Expression<Func<T, Boolean>> predicate, Action<T> valueUpdater)
    {
        return await Task.FromResult(UpdateMany(predicate, valueUpdater));
    }

    public async Task<Boolean> DeleteAsync(Expression<Func<T, Boolean>> predicate)
    {
        return await Task.FromResult(Delete(predicate));
    }

    public async Task<Int32> DeleteManyAsync(Expression<Func<T, Boolean>> predicate)
    {
        return await Task.FromResult(DeleteMany(predicate));
    }
}