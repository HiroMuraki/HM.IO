namespace HM.IO.Providers;

public static class FilesProviderExtensions
{
    #region Use
    public static EntryPathsProvider Use<T>(this EntryPathsProvider self, T component)
        where T : notnull
    {
        switch (typeof(T))
        {
            case IDirectoryIO:
                self.UseDirectoryIO((IDirectoryIO)component);
                break;
            case IErrorHandler:
                self.UseErrorHandler((IErrorHandler)component);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(component));
        }

        return self;
    }
    #endregion

    #region IncludeDirectory
    public static EntryPathsProvider IncludeDirectory(this EntryPathsProvider self, EntryPath path)
    {
        return self.IncludeDirectory(new SearchingDirectory(path));
    }

    public static EntryPathsProvider IncludeDirectory(this EntryPathsProvider self, String path)
        => IncludeDirectory(self, EntryPath.CreateFromPath(path));

    public static EntryPathsProvider IncludeDirectories(this EntryPathsProvider self, IEnumerable<SearchingDirectory> searchingDirectories)
    {
        foreach (SearchingDirectory dir in searchingDirectories)
        {
            self.IncludeDirectory(dir);
        }

        return self;
    }

    public static EntryPathsProvider IncludeDirectories(this EntryPathsProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath path in entryPaths)
        {
            IncludeDirectory(self, path);
        }

        return self;
    }

    public static EntryPathsProvider IncludeDirectories(this EntryPathsProvider self, IEnumerable<String> paths)
        => IncludeDirectories(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region IncludeFile
    public static EntryPathsProvider IncludeFile(this EntryPathsProvider self, EntryPath path)
    {
        return self.IncludeFile(new SearchingFile(path));
    }

    public static EntryPathsProvider IncludeFile(this EntryPathsProvider self, String path)
        => IncludeFile(self, EntryPath.CreateFromPath(path));

    public static EntryPathsProvider IncludeFiles(this EntryPathsProvider self, IEnumerable<SearchingFile> searchingFiles)
    {
        foreach (SearchingFile dir in searchingFiles)
        {
            self.IncludeFile(dir);
        }

        return self;
    }

    public static EntryPathsProvider IncludeFiles(this EntryPathsProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath path in entryPaths)
        {
            IncludeFile(self, path);
        }

        return self;
    }

    public static EntryPathsProvider IncludeFiles(this EntryPathsProvider self, IEnumerable<String> paths)
        => IncludeFiles(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region ExcludeDirectory
    public static EntryPathsProvider ExcludeDirectory(this EntryPathsProvider self, EntryPath path)
    {
        return self.ExcludeDirectory(new SearchingDirectory(path));
    }

    public static EntryPathsProvider ExcludeDirectory(this EntryPathsProvider self, String path)
        => ExcludeDirectory(self, EntryPath.CreateFromPath(path));

    public static EntryPathsProvider ExcludeDirectories(this EntryPathsProvider self, IEnumerable<SearchingDirectory> searchingDirectories)
    {
        foreach (SearchingDirectory dir in searchingDirectories)
        {
            self.ExcludeDirectory(dir);
        }

        return self;
    }

    public static EntryPathsProvider ExcludeDirectories(this EntryPathsProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath path in entryPaths)
        {
            ExcludeDirectory(self, path);
        }

        return self;
    }

    public static EntryPathsProvider ExcludeDirectories(this EntryPathsProvider self, IEnumerable<String> paths)
        => ExcludeDirectories(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region ExcludeFile
    public static EntryPathsProvider ExcludeFile(this EntryPathsProvider self, EntryPath path)
    {
        return self.ExcludeFile(new SearchingFile(path));
    }

    public static EntryPathsProvider ExcludeFile(this EntryPathsProvider self, String path)
        => ExcludeFile(self, EntryPath.CreateFromPath(path));

    public static EntryPathsProvider ExcludeFiles(this EntryPathsProvider self, IEnumerable<SearchingFile> searchingFiles)
    {
        foreach (SearchingFile dir in searchingFiles)
        {
            self.ExcludeFile(dir);
        }

        return self;
    }

    public static EntryPathsProvider ExcludeFiles(this EntryPathsProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath path in entryPaths)
        {
            ExcludeFile(self, path);
        }

        return self;
    }

    public static EntryPathsProvider ExcludeFiles(this EntryPathsProvider self, IEnumerable<String> paths)
        => ExcludeFiles(self, paths.Select(EntryPath.CreateFromPath));
    #endregion
}
