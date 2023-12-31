namespace HM.IO.Providers;

public static class FilesProviderExtensions
{
    #region IncludeDirectory
    public static FilesProvider IncludeDirectory(this FilesProvider self, EntryPath path)
    {
        return self.IncludeDirectory(new SearchingDirectory(path));
    }

    public static FilesProvider IncludeDirectory(this FilesProvider self, String path)
        => IncludeDirectory(self, path);

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
        => IncludeFile(self, path);

    public static FilesProvider IncludeFiles(this FilesProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath path in entryPaths)
        {
            IncludeDirectory(self, path);
        }

        return self;
    }

    public static FilesProvider IncludeFiles(this FilesProvider self, IEnumerable<String> paths)
        => IncludeDirectories(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region ExcludeDirectory
    public static FilesProvider ExcludeDirectory(this FilesProvider self, EntryPath path)
    {
        return self.IncludeDirectory(new SearchingDirectory(path));
    }

    public static FilesProvider ExcludeDirectory(this FilesProvider self, String path)
        => IncludeDirectory(self, path);

    public static FilesProvider ExcludeDirectories(this FilesProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath path in entryPaths)
        {
            IncludeDirectory(self, path);
        }

        return self;
    }

    public static FilesProvider ExcludeDirectories(this FilesProvider self, IEnumerable<String> paths)
        => IncludeDirectories(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region ExcludeFile
    public static FilesProvider ExcludeFile(this FilesProvider self, EntryPath path)
    {
        return self.IncludeFile(new SearchingFile(path));
    }

    public static FilesProvider ExcludeFile(this FilesProvider self, String path)
        => IncludeFile(self, path);

    public static FilesProvider ExcludeFiles(this FilesProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath path in entryPaths)
        {
            IncludeDirectory(self, path);
        }

        return self;
    }

    public static FilesProvider ExcludeFiles(this FilesProvider self, IEnumerable<String> paths)
        => IncludeDirectories(self, paths.Select(EntryPath.CreateFromPath));
    #endregion
}
