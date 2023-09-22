﻿namespace HM.IO;

public sealed class SearchingDirectory
{
    public EntryPath BaseDirectroy { get; init; }

    public Boolean RecurseSubdirectories { get; init; } = true;

    public Int32 MaxRecursionDepth { get; init; } = Int32.MaxValue;

    public Boolean IgnoreIfNotExists { get; init; } = true;

    public SearchingDirectory(String path)
    {
        BaseDirectroy = EntryPath.CreateFromPath(path);
    }
}
