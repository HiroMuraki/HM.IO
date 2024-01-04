namespace HM.IO;

/// <summary>
/// Represents a readonly structure for specifying a directory to be searched, including optional parameters.
/// </summary>
public readonly struct SearchingDirectory
{
    /// <include file='Providers/SearchingDirectory.xml' path='SearchingDirectory/Properties/Instance[@name="Path"]/*' />
    public EntryPath Path { get; init; }

    /// <include file='Providers/SearchingDirectory.xml' path='SearchingDirectory/Properties/Instance[@name="RecurseSubdirectories"]/*' />
    public Boolean RecurseSubdirectories { get; init; } = false;

    /// <include file='Providers/SearchingDirectory.xml' path='SearchingDirectory/Properties/Instance[@name="RecurseSubdirectories"]/*' />
    public Int32 MaxRecursionDepth { get; init; } = Int32.MaxValue;

    /// <include file='Providers/SearchingDirectory.xml' path='SearchingDirectory/Properties/Instance[@name="IgnoreIfNotExists"]/*' />
    public Boolean IgnoreIfNotExists { get; init; } = false;

    /// <include file='Providers/SearchingDirectory.xml' path='SearchingDirectory/Ctors/Ctor[@name="SearchingDirectory[EntryPath]"]/*' />
    public SearchingDirectory(EntryPath entryPath)
    {
        Path = entryPath;
    }
}
