namespace HM.IO;

public readonly struct SearchingFile
{
    public EntryPath Path { get; init; }

    public Boolean IgnoreIfNotExists { get; init; } = false;

    public SearchingFile(EntryPath path)
    {
        Path = path;
    }
}
