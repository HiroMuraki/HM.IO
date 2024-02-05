namespace HM.IO.Providers;

/// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Class[@name="EntryPathsProviderExtensions"]/*' />
public static class EntryPathsProviderExtensions
{
    #region Use
    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="Use[EntryPathsProvider,T]"]/*' />
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
    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="IncludeDirectory[EntryPathsProvider,EntryPath]"]/*' />
    public static EntryPathsProvider IncludeDirectory(this EntryPathsProvider self, EntryPath path)
    {
        return self.IncludeDirectory(new SearchingDirectory(path));
    }

    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="IncludeDirectory[EntryPathsProvider,String]"]/*' />
    public static EntryPathsProvider IncludeDirectory(this EntryPathsProvider self, String path)
        => IncludeDirectory(self, EntryPath.CreateFromPath(path));

    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="IncludeDirectories[EntryPathsProvider,IEnumerable&lt;SearchingDirectory&gt;]"]/*' />
    public static EntryPathsProvider IncludeDirectories(this EntryPathsProvider self, IEnumerable<SearchingDirectory> searchingDirectories)
    {
        foreach (SearchingDirectory dir in searchingDirectories)
        {
            self.IncludeDirectory(dir);
        }

        return self;
    }

    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="IncludeDirectories[EntryPathsProvider,IEnumerable&lt;EntryPath&gt;]"]/*' />
    public static EntryPathsProvider IncludeDirectories(this EntryPathsProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath path in entryPaths)
        {
            IncludeDirectory(self, path);
        }

        return self;
    }

    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="IncludeDirectories[EntryPathsProvider,IEnumerable&lt;String&gt;]"]/*' />
    public static EntryPathsProvider IncludeDirectories(this EntryPathsProvider self, IEnumerable<String> paths)
        => IncludeDirectories(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region IncludeFile
    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="IncludeFile[EntryPathsProvider,EntryPath]"]/*' />
    public static EntryPathsProvider IncludeFile(this EntryPathsProvider self, EntryPath path)
    {
        return self.IncludeFile(new SearchingFile(path));
    }

    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="IncludeFile[EntryPathsProvider,String]"]/*' />
    public static EntryPathsProvider IncludeFile(this EntryPathsProvider self, String path)
        => IncludeFile(self, EntryPath.CreateFromPath(path));

    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="IncludeFiles[EntryPathsProvider,IEnumerable&lt;SearchingFile&gt;]"]/*' />
    public static EntryPathsProvider IncludeFiles(this EntryPathsProvider self, IEnumerable<SearchingFile> searchingFiles)
    {
        foreach (SearchingFile dir in searchingFiles)
        {
            self.IncludeFile(dir);
        }

        return self;
    }

    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="IncludeFiles[EntryPathsProvider,IEnumerable&lt;EntryPath&gt;]"]/*' />
    public static EntryPathsProvider IncludeFiles(this EntryPathsProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath path in entryPaths)
        {
            IncludeFile(self, path);
        }

        return self;
    }

    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="IncludeFiles[EntryPathsProvider,IEnumerable&lt;String&gt;]"]/*' />
    public static EntryPathsProvider IncludeFiles(this EntryPathsProvider self, IEnumerable<String> paths)
        => IncludeFiles(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region ExcludeDirectory
    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="ExcludeDirectory[EntryPathsProvider,EntryPath]"]/*' />
    public static EntryPathsProvider ExcludeDirectory(this EntryPathsProvider self, EntryPath path)
    {
        return self.ExcludeDirectory(new SearchingDirectory(path));
    }

    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="ExcludeDirectory[EntryPathsProvider,String]"]/*' />
    public static EntryPathsProvider ExcludeDirectory(this EntryPathsProvider self, String path)
        => ExcludeDirectory(self, EntryPath.CreateFromPath(path));

    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="ExcludeDirectories[EntryPathsProvider,IEnumerable&lt;SearchingDirectory&gt;]"]/*' />
    public static EntryPathsProvider ExcludeDirectories(this EntryPathsProvider self, IEnumerable<SearchingDirectory> searchingDirectories)
    {
        foreach (SearchingDirectory dir in searchingDirectories)
        {
            self.ExcludeDirectory(dir);
        }

        return self;
    }

    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="ExcludeDirectories[EntryPathsProvider,IEnumerable&lt;EntryPath&gt;]"]/*' />
    public static EntryPathsProvider ExcludeDirectories(this EntryPathsProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath path in entryPaths)
        {
            ExcludeDirectory(self, path);
        }

        return self;
    }

    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="ExcludeDirectories[EntryPathsProvider,IEnumerable&lt;String&gt;]"]/*' />
    public static EntryPathsProvider ExcludeDirectories(this EntryPathsProvider self, IEnumerable<String> paths)
        => ExcludeDirectories(self, paths.Select(EntryPath.CreateFromPath));
    #endregion

    #region ExcludeFile
    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="ExcludeFile[EntryPathsProvider,EntryPath]"]/*' />
    public static EntryPathsProvider ExcludeFile(this EntryPathsProvider self, EntryPath path)
    {
        return self.ExcludeFile(new SearchingFile(path));
    }

    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="ExcludeFile[EntryPathsProvider,String]"]/*' />
    public static EntryPathsProvider ExcludeFile(this EntryPathsProvider self, String path)
        => ExcludeFile(self, EntryPath.CreateFromPath(path));

    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="ExcludeFiles[EntryPathsProvider,IEnumerable&lt;SearchingFile&gt;]"]/*' />
    public static EntryPathsProvider ExcludeFiles(this EntryPathsProvider self, IEnumerable<SearchingFile> searchingFiles)
    {
        foreach (SearchingFile dir in searchingFiles)
        {
            self.ExcludeFile(dir);
        }

        return self;
    }

    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="ExcludeFiles[EntryPathsProvider,IEnumerable&lt;EntryPath&gt;]"]/*' />
    public static EntryPathsProvider ExcludeFiles(this EntryPathsProvider self, IEnumerable<EntryPath> entryPaths)
    {
        foreach (EntryPath path in entryPaths)
        {
            ExcludeFile(self, path);
        }

        return self;
    }

    /// <include file='Providers/EntryPathsProviderExtensions.xml' path='EntryPathsProviderExtensions/Methods/Static[@name="ExcludeFiles[EntryPathsProvider,IEnumerable&lt;String&gt;]"]/*' />
    public static EntryPathsProvider ExcludeFiles(this EntryPathsProvider self, IEnumerable<String> paths)
        => ExcludeFiles(self, paths.Select(EntryPath.CreateFromPath));
    #endregion
}
