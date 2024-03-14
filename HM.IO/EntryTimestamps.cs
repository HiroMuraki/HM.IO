namespace HM.IO;

public readonly struct EntryTimestamps
{
    public DateTime CreationTime { get; init; }

    public DateTime LastWriteTime { get; init; }

    public DateTime LastAccessTime { get; init; }
}