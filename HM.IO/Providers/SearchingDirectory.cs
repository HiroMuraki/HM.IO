namespace HM.IO;

public readonly struct SearchingDirectory
{
    public EntryPath Path { get; init; }

    public Boolean RecurseSubdirectories { get; init; } = false;

    public Int32 MaxRecursionDepth { get; init; } = Int32.MaxValue;

    public Boolean IgnoreIfNotExists { get; init; } = false;

    public SearchingDirectory(EntryPath path) : this(path, false)
    {
    }

    public SearchingDirectory(EntryPath path, Boolean recurseSubdirectories)
    {
        Path = path;
        if (recurseSubdirectories)
        {
            RecurseSubdirectories = true;
            MaxRecursionDepth = Int32.MaxValue;
        }
    }

    public SearchingDirectory(EntryPath path, Int32 maxRecurseLevel)
    {
        Path = path;
        RecurseSubdirectories = maxRecurseLevel > 0;
        MaxRecursionDepth = maxRecurseLevel;
    }
}
