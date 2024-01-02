#if PREVIEW
namespace HM.IO.Providers;

public static class DirectoriesProviderExtensions
{
    #region Use
    public static DirectoriesProvider Use<T>(this DirectoriesProvider self, T component)
        where T : notnull
    {
        if (typeof(T) == typeof(IDirectoryIO))
        {
            self.UseDirectoryIO((IDirectoryIO)component);
        }
        else if (typeof(T) == typeof(IErrorHandler))
        {
            self.UseErrorHandler((IErrorHandler)component);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(component));
        }

        return self;
    }
    #endregion

    #region IncludeDirectory
    public static DirectoriesProvider IncludeDirectory(this DirectoriesProvider self, EntryPath path)
    {
        return self.IncludeDirectory(new SearchingDirectory(path));
    }

    public static DirectoriesProvider IncludeDirectory(this DirectoriesProvider self, String path)
        => IncludeDirectory(self, EntryPath.CreateFromPath(path));

    public static DirectoriesProvider IncludeDirectories(this DirectoriesProvider self, IEnumerable<SearchingDirectory> searchingDirectories)
    {
        foreach (SearchingDirectory dir in searchingDirectories)
        {
            self.IncludeDirectory(dir);
        }

        return self;
    }

    public static DirectoriesProvider IncludeDirectories(this DirectoriesProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath path in entryPaths)
        {
            IncludeDirectory(self, path);
        }

        return self;
    }

    public static DirectoriesProvider IncludeDirectories(this DirectoriesProvider self, IEnumerable<String> paths)
        => IncludeDirectories(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region ExcludeDirectory
    public static DirectoriesProvider ExcludeDirectory(this DirectoriesProvider self, EntryPath path)
    {
        return self.ExcludeDirectory(new SearchingDirectory(path));
    }

    public static DirectoriesProvider ExcludeDirectory(this DirectoriesProvider self, String path)
        => ExcludeDirectory(self, EntryPath.CreateFromPath(path));

    public static DirectoriesProvider ExcludeDirectories(this DirectoriesProvider self, IEnumerable<SearchingDirectory> searchingDirectories)
    {
        foreach (SearchingDirectory dir in searchingDirectories)
        {
            self.ExcludeDirectory(dir);
        }

        return self;
    }

    public static DirectoriesProvider ExcludeDirectories(this DirectoriesProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath path in entryPaths)
        {
            ExcludeDirectory(self, path);
        }

        return self;
    }

    public static DirectoriesProvider ExcludeDirectories(this DirectoriesProvider self, IEnumerable<String> paths)
        => ExcludeDirectories(self, paths.Select(EntryPath.CreateFromPath));
    #endregion
}
#endif