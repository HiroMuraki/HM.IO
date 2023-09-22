namespace HM.IO;

public sealed class SearchingFile
{
    public EntryPath FilePath { get; init; }

    public Boolean IgnoreIfNotExists { get; init; } = true;

    public SearchingFile(String filePath)
    {
        FilePath = EntryPath.CreateFromPath(filePath);
    }
}
