namespace HM.IO.Providers;

public static class DirectoriesProviderExtensions
{
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
        return self.IncludeDirectory(new SearchingDirectory(path));
    }

    public static DirectoriesProvider ExcludeDirectory(this DirectoriesProvider self, String path)
        => IncludeDirectory(self, EntryPath.CreateFromPath(path));

    public static DirectoriesProvider ExcludeDirectories(this DirectoriesProvider self, IEnumerable<SearchingDirectory> searchingDirectories)
    {
        foreach (SearchingDirectory dir in searchingDirectories)
        {
            self.IncludeDirectory(dir);
        }

        return self;
    }

    public static DirectoriesProvider ExcludeDirectories(this DirectoriesProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath path in entryPaths)
        {
            IncludeDirectory(self, path);
        }

        return self;
    }

    public static DirectoriesProvider ExcludeDirectories(this DirectoriesProvider self, IEnumerable<String> paths)
        => ExcludeDirectories(self, paths.Select(EntryPath.CreateFromPath));
    #endregion
}