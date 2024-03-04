//using HM.Common;
//using Microsoft.EntityFrameworkCore;
//using System.Linq.Expressions;

//namespace AppDataBase;

//public class JsonDatabase<T> :
//    IDatabase<T>,
//    IDisposable
//    where T : class
//{
//    public async Task InitializeAsync()
//    {
//        if (_isInitialized)
//        {
//            return;
//        }

//        await _context.Database.EnsureCreatedAsync();
//        _isInitialized = true;
//    }

//    public T? Query(Expression<Func<T, bool>> predicate)
//    {
//        return ParallelSafe.Run(() =>
//        {
//            CheckStates();

//            return _context.Set<T>().AsNoTracking().Where(predicate).SingleOrDefault();
//        }, _readerWriterLockSlim.EnterReadLock, _readerWriterLockSlim.ExitReadLock);
//    }

//    public IEnumerable<T> QueryMany(Expression<Func<T, bool>> predicate)
//    {
//        return ParallelSafe.Run(() =>
//        {
//            CheckStates();

//            return _context.Set<T>().AsNoTracking().Where(predicate);
//        }, _readerWriterLockSlim.EnterReadLock, _readerWriterLockSlim.ExitReadLock);
//    }

//    public void Add(T item)
//    {
//        ParallelSafe.Run(() =>
//        {
//            CheckStates();

//            _context.Set<T>().Add(item);
//        }, _readerWriterLockSlim.EnterWriteLock, _readerWriterLockSlim.ExitWriteLock);
//    }

//    public void AddMany(IEnumerable<T> items)
//    {
//        ParallelSafe.Run(() =>
//        {
//            CheckStates();

//            _context.Set<T>().AddRange(items);
//        }, _readerWriterLockSlim.EnterWriteLock, _readerWriterLockSlim.ExitWriteLock);
//    }

//    public bool Update(Expression<Func<T, bool>> predicate, Action<T> valueUpdater)
//    {
//        return ParallelSafe.Run(() =>
//        {
//            CheckStates();

//            DbSet<T> dbSet = _context.Set<T>();
//            T? targetValue = dbSet.Where(predicate).SingleOrDefault();
//            if (targetValue is null)
//            {
//                return false;
//            }

//            valueUpdater(targetValue);
//            return true;
//        }, _readerWriterLockSlim.EnterWriteLock, _readerWriterLockSlim.ExitWriteLock);
//    }

//    public int UpdateMany(Expression<Func<T, bool>> predicate, Action<T> valueUpdater)
//    {
//        return ParallelSafe.Run(() =>
//        {
//            CheckStates();

//            DbSet<T> dbSet = _context.Set<T>();
//            IQueryable<T> targetValues = dbSet.Where(predicate);
//            int count = 0;

//            foreach (T targetValue in targetValues)
//            {
//                valueUpdater(targetValue);
//                count++;
//            }

//            return count;
//        }, _readerWriterLockSlim.EnterWriteLock, _readerWriterLockSlim.ExitWriteLock);
//    }

//    public bool Delete(Expression<Func<T, bool>> predicate)
//    {
//        return ParallelSafe.Run(() =>
//        {
//            CheckStates();

//            T? targetItem = Query(predicate);
//            if (targetItem is null)
//            {
//                return false;
//            }

//            _context.Set<T>().RemoveRange(targetItem);
//            return true;
//        }, _readerWriterLockSlim.EnterWriteLock, _readerWriterLockSlim.ExitWriteLock);
//    }

//    public int DeleteMany(Expression<Func<T, bool>> predicate)
//    {
//        return ParallelSafe.Run(() =>
//        {
//            CheckStates();

//            IEnumerable<T> targetItems = _context.Set<T>().Where(predicate);
//            int count = 0;

//            foreach (T item in targetItems)
//            {
//                _context.Set<T>().Remove(item);
//                count++;
//            }

//            return count;
//        }, _readerWriterLockSlim.EnterWriteLock, _readerWriterLockSlim.ExitWriteLock);
//    }

//    public async Task SaveChangesAsync()
//    {
//        CheckStates();

//        await _context.SaveChangesAsync();
//    }

//    ~DatabaseBase()
//    {
//        Dispose(disposing: false);
//    }

//    public void Dispose()
//    {
//        Dispose(disposing: true);
//        GC.SuppressFinalize(this);
//    }

//    #region NonPublic
//    protected virtual void Dispose(bool disposing)
//    {
//        if (_isDisposed)
//        {
//            return;
//        }

//        if (disposing)
//        {
//            _readerWriterLockSlim.Dispose();
//            _readerWriterLockSlim = null!;
//            if (_context is IDisposable disposable)
//            {
//                disposable.Dispose();
//            }
//            _context = null!;
//        }

//        _isDisposed = true;
//    }
//    private DatabaseBase(IContext dbContext)
//    {
//        _context = dbContext;
//    }
//    private bool _isInitialized;
//    private bool _isDisposed;
//    private ReaderWriterLockSlim _readerWriterLockSlim = new();
//    private void CheckStates()
//    {
//        if (!_isInitialized)
//        {
//            throw new InvalidOperationException("Database was not initialized");
//        }

//        if (_isDisposed)
//        {
//            throw new InvalidOperationException("Database was disposed");
//        }
//    }
//    #endregion
//}
