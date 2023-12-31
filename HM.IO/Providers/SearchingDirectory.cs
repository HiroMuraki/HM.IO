namespace HM.IO;

public readonly struct SearchingDirectory
{
    public EntryPath Path { get; init; }

    public Boolean RecurseSubdirectories { get; init; } = false;

    public Int32 MaxRecursionDepth { get; init; } = Int32.MaxValue;

    public Boolean IgnoreIfNotExists { get; init; } = false;

    public SearchingDirectory(EntryPath path)
    {
        Path = path;
    }
}
