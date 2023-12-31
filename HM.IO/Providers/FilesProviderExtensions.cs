namespace HM.IO.Providers;

public static class FilesProviderExtensions
{
    #region IncludeDirectory
    public static FilesProvider IncludeDirectory(this FilesProvider self, String path)
    {
        return self.IncludeDirectory(EntryPath.CreateFromPath(path));
    }

    public static FilesProvider IncludeDirectories(this FilesProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath item in entryPaths)
        {
            self.IncludeDirectory(item);
        }

        return self;
    }

    public static FilesProvider IncludeDirectories(this FilesProvider self, IEnumerable<String> paths)
        => IncludeDirectories(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region IncludeFile
    public static FilesProvider IncludeFile(this FilesProvider self, String path)
    {
        return self.IncludeFile(EntryPath.CreateFromPath(path));
    }

    public static FilesProvider IncludeFiles(this FilesProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath item in entryPaths)
        {
            self.IncludeFile(item);
        }

        return self;
    }

    public static FilesProvider IncludeFiles(this FilesProvider self, IEnumerable<String> paths)
        => IncludeFiles(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region ExcludeDirectory
    public static FilesProvider ExcludeDirectory(this FilesProvider self, String path)
    {
        return self.ExcludeDirectory(EntryPath.CreateFromPath(path));
    }

    public static FilesProvider ExcludeDirectories(this FilesProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath item in entryPaths)
        {
            self.ExcludeDirectory(item);
        }

        return self;
    }

    public static FilesProvider ExcludeDirectories(this FilesProvider self, IEnumerable<String> paths)
        => ExcludeDirectories(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region ExcludeFile
    public static FilesProvider ExcludeFile(this FilesProvider self, String path)
    {
        return self.ExcludeFile(EntryPath.CreateFromPath(path));
    }

    public static FilesProvider ExcludeFiles(this FilesProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath item in entryPaths)
        {
            self.ExcludeFile(item);
        }

        return self;
    }

    public static FilesProvider ExcludeFiles(this FilesProvider self, IEnumerable<String> paths)
        => ExcludeFiles(self, paths.Select(EntryPath.CreateFromPath));
    #endregion
}
