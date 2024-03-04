﻿namespace HM.IO.Previews.Entry;

public interface IDirectory
{
    DirectoryPath Path { get; }

    Boolean Exists { get; }

    EntryTimestamps Timestamps { get; set; }

    EntryAttributes Attributes { get; set; }

    IEnumerable<IFile> EnumerateFiles(EnumerationOptions enumerationOptions);

    IEnumerable<IDirectory> EnumerateDirectory(EnumerationOptions enumerationOptions);
}