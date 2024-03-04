namespace HM.IO.Providers;

public static class EntryPathsProviderExtensions
{
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

    public static EntryPathsProvider IncludeDirectories(this EntryPathsProvider self, IEnumerable<SearchingDirectory> searchingDirectories)
    {
        foreach (SearchingDirectory path in searchingDirectories)
        {
            self.IncludeDirectory(path);
        }

        return self;
    }

    public static EntryPathsProvider IncludeFiles(this EntryPathsProvider self, IEnumerable<SearchingFile> searchingFiles)
    {
        foreach (SearchingFile dir in searchingFiles)
        {
            self.IncludeFile(dir);
        }

        return self;
    }

    public static EntryPathsProvider ExcludeDirectories(this EntryPathsProvider self, IEnumerable<SearchingDirectory> searchingDirectories)
    {
        foreach (SearchingDirectory dir in searchingDirectories)
        {
            self.ExcludeDirectory(dir);
        }

        return self;
    }

    public static EntryPathsProvider ExcludeFiles(this EntryPathsProvider self, IEnumerable<SearchingFile> searchingFiles)
    {
        foreach (SearchingFile dir in searchingFiles)
        {
            self.ExcludeFile(dir);
        }

        return self;
    }
}
