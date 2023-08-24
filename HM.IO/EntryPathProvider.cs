using HM.IO.RoutedItems;
using System.Diagnostics.CodeAnalysis;

namespace HM.IO;

public abstract class EntryPathProvider :
    IItemsProvider<EntryPath>
{
    protected readonly struct CompressedEntryPath :
        IEquatable<CompressedEntryPath>,
        IComparable<CompressedEntryPath>
    {
        public Int32[] Routes => _routes;

        public override readonly String ToString()
        {
            return RoutedItemHelper.ToString(in _routes);
        }

        public override Int32 GetHashCode()
        {
            return RoutedItemHelper.GetHashCode(in _routes);
        }

        public override Boolean Equals([NotNullWhen(true)] Object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((CompressedEntryPath)obj);
        }

        public Boolean Equals(CompressedEntryPath other)
        {
            return this == other;
        }

        public Int32 CompareTo(CompressedEntryPath other)
        {
            return RoutedItemHelper.Compare(in _routes, in other._routes, Comparer<Int32>.Default);
        }

        public static Boolean operator ==(CompressedEntryPath left, CompressedEntryPath right)
        {
            return RoutedItemHelper.Equals(in left._routes, in right._routes, EqualityComparer<Int32>.Default);
        }

        public static Boolean operator !=(CompressedEntryPath left, CompressedEntryPath right)
        {
            return !(left == right);
        }

        public CompressedEntryPath(Int32 size)
        {
            _routes = new Int32[size];
        }

        #region NonPublic
        private readonly Int32[] _routes;
        #endregion
    }

    protected class EntryPathCompressor
    {
        public CompressedEntryPath Compress(EntryPath entryPath)
        {
            if (_reversedRouteIdMap is not null)
            {
                throw new InvalidOperationException("Unable to call method `Compress` after `Restore` called");
            }

            var compressedEntryPath = new CompressedEntryPath(entryPath.Length);
            for (Int32 i = 0; i < entryPath.Length; i++)
            {
                compressedEntryPath.Routes[i] = GetRouteId(entryPath[i]);
            }
            return compressedEntryPath;
        }

        public EntryPath Restore(CompressedEntryPath compressedRoutedPath)
        {
            _reversedRouteIdMap ??= _routeIdMap.ToDictionary(k => k.Value, v => v.Key);

            String[] routes = new String[compressedRoutedPath.Routes.Length];

            for (Int32 i = 0; i < compressedRoutedPath.Routes.Length; i++)
            {
                routes[i] = GetRoute(compressedRoutedPath.Routes[i]);
            }

            return new EntryPath(routes);
        }

        #region NonPublic
        private readonly Dictionary<String, Int32> _routeIdMap = new();
        private Dictionary<Int32, String>? _reversedRouteIdMap;
        private Int32 _currentId = 0;
        private Int32 GetRouteId(String path)
        {
            if (!_routeIdMap.TryGetValue(path, out Int32 id))
            {
                _routeIdMap[path] = _currentId;
                id = _currentId;
                _currentId++;
            }

            return id;
        }
        private String GetRoute(Int32 pathId)
        {
            if (_reversedRouteIdMap!.TryGetValue(pathId, out String? route))
            {
                return route;
            }
            else
            {
                throw new ArgumentException($"Unable to find route for path id {pathId}");
            }
        }
        #endregion
    }

    public IDirectoryIO DirectoryIO { get; set; } = new DirectoryIO();
    public IEntryPathEqualityComparer EntryPathComparer { get; } = IO.EntryPathEqualityComparer.Default;

    public abstract IEnumerable<EntryPath> EnumerateItems();

    #region NonPublic
    protected static List<EntryPath> SelectNotEmptyAsEntryPath(IEnumerable<String> items)
    {
        return items
            .Where(x => !String.IsNullOrWhiteSpace(x))
            .Select(EntryPath.CreateFromPath)
            .ToList();
    }
    #endregion
}
