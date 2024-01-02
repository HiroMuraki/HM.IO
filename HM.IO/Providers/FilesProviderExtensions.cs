namespace HM.IO.Providers;

public static class FilesProviderExtensions
{
    #region Use
    public static FilesProvider Use<T>(this FilesProvider self, T component)
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
    public static FilesProvider IncludeDirectory(this FilesProvider self, EntryPath path)
    {
        return self.IncludeDirectory(new SearchingDirectory(path));
    }

    public static FilesProvider IncludeDirectory(this FilesProvider self, String path)
        => IncludeDirectory(self, EntryPath.CreateFromPath(path));

    public static FilesProvider IncludeDirectories(this FilesProvider self, IEnumerable<SearchingDirectory> searchingDirectories)
    {
        foreach (SearchingDirectory dir in searchingDirectories)
        {
            self.IncludeDirectory(dir);
        }

        return self;
    }

    public static FilesProvider IncludeDirectories(this FilesProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath path in entryPaths)
        {
            IncludeDirectory(self, path);
        }

        return self;
    }

    public static FilesProvider IncludeDirectories(this FilesProvider self, IEnumerable<String> paths)
        => IncludeDirectories(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region IncludeFile
    public static FilesProvider IncludeFile(this FilesProvider self, EntryPath path)
    {
        return self.IncludeFile(new SearchingFile(path));
    }

    public static FilesProvider IncludeFile(this FilesProvider self, String path)
        => IncludeFile(self, EntryPath.CreateFromPath(path));

    public static FilesProvider IncludeFiles(this FilesProvider self, IEnumerable<SearchingFile> searchingFiles)
    {
        foreach (SearchingFile dir in searchingFiles)
        {
            self.IncludeFile(dir);
        }

        return self;
    }

    public static FilesProvider IncludeFiles(this FilesProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath path in entryPaths)
        {
            IncludeFile(self, path);
        }

        return self;
    }

    public static FilesProvider IncludeFiles(this FilesProvider self, IEnumerable<String> paths)
        => IncludeFiles(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region ExcludeDirectory
    public static FilesProvider ExcludeDirectory(this FilesProvider self, EntryPath path)
    {
        return self.ExcludeDirectory(new SearchingDirectory(path));
    }

    public static FilesProvider ExcludeDirectory(this FilesProvider self, String path)
        => ExcludeDirectory(self, EntryPath.CreateFromPath(path));

    public static FilesProvider ExcludeDirectories(this FilesProvider self, IEnumerable<SearchingDirectory> searchingDirectories)
    {
        foreach (SearchingDirectory dir in searchingDirectories)
        {
            self.ExcludeDirectory(dir);
        }

        return self;
    }

    public static FilesProvider ExcludeDirectories(this FilesProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath path in entryPaths)
        {
            ExcludeDirectory(self, path);
        }

        return self;
    }

    public static FilesProvider ExcludeDirectories(this FilesProvider self, IEnumerable<String> paths)
        => ExcludeDirectories(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region ExcludeFile
    public static FilesProvider ExcludeFile(this FilesProvider self, EntryPath path)
    {
        return self.ExcludeFile(new SearchingFile(path));
    }

    public static FilesProvider ExcludeFile(this FilesProvider self, String path)
        => ExcludeFile(self, EntryPath.CreateFromPath(path));

    public static FilesProvider ExcludeFiles(this FilesProvider self, IEnumerable<SearchingFile> searchingFiles)
    {
        foreach (SearchingFile dir in searchingFiles)
        {
            self.ExcludeFile(dir);
        }

        return self;
    }

    public static FilesProvider ExcludeFiles(this FilesProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath path in entryPaths)
        {
            ExcludeFile(self, path);
        }

        return self;
    }

    public static FilesProvider ExcludeFiles(this FilesProvider self, IEnumerable<String> paths)
        => ExcludeFiles(self, paths.Select(EntryPath.CreateFromPath));
    #endregion
}
