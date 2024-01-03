namespace HM.IO;

/// <include file='Docs/Providers/SearchingFile.xml' path='SearchingFile/Class[@name="SearchingFile"]/*' />
public readonly struct SearchingFile
{
    /// <include file='Docs/Providers/SearchingFile.xml' path='SearchingFile/Properties/Instance[@name="Path"]/*' />
    public EntryPath Path { get; init; }

    /// <include file='Docs/Providers/SearchingFile.xml' path='SearchingFile/Properties/Instance[@name="IgnoreIfNotExists"]/*' />
    public Boolean IgnoreIfNotExists { get; init; } = false;

    /// <include file='Docs/Providers/SearchingFile.xml' path='SearchingFile/Ctors/Ctor[@name="SearchingFile[]"]/*' />
    public SearchingFile(EntryPath entryPath)
    {
        Path = entryPath;
    }
}