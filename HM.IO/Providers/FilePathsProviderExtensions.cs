namespace HM.IO.Providers;

public static class FilePathsProviderExtensions
{
    public static LocalFilePathsProvider Use<T>(this LocalFilePathsProvider self, T component)
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

    public static LocalFilePathsProvider IncludeDirectories(this LocalFilePathsProvider self, IEnumerable<SearchingDirectory> searchingDirectories)
    {
        foreach (SearchingDirectory path in searchingDirectories)
        {
            self.IncludeDirectory(path);
        }

        return self;
    }

    public static LocalFilePathsProvider IncludeFiles(this LocalFilePathsProvider self, IEnumerable<SearchingFile> searchingFiles)
    {
        foreach (SearchingFile dir in searchingFiles)
        {
            self.IncludeFile(dir);
        }

        return self;
    }

    public static LocalFilePathsProvider ExcludeDirectories(this LocalFilePathsProvider self, IEnumerable<SearchingDirectory> searchingDirectories)
    {
        foreach (SearchingDirectory dir in searchingDirectories)
        {
            self.ExcludeDirectory(dir);
        }

        return self;
    }

    public static LocalFilePathsProvider ExcludeFiles(this LocalFilePathsProvider self, IEnumerable<SearchingFile> searchingFiles)
    {
        foreach (SearchingFile dir in searchingFiles)
        {
            self.ExcludeFile(dir);
        }

        return self;
    }
}
