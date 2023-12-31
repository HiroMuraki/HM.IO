﻿namespace HM.IO;

/// <include file='IDirectoryIO.xml' path='IDirectoryIO/Class[@name="IDirectoryIO"]/*' />
public interface IDirectoryIO
{
    /// <include file='IDirectoryIO.xml' path='IDirectoryIO/Methods/Instance[@name="Exists[EntryPath]"]/*' />
    Boolean Exists(EntryPath entryPath);

    /// <include file='IDirectoryIO.xml' path='IDirectoryIO/Methods/Instance[@name="EnumerateFiles[EntryPath,EnumerationOptions]"]/*' />
    IEnumerable<EntryPath> EnumerateFiles(EntryPath path, EnumerationOptions enumerationOptions);

    /// <include file='IDirectoryIO.xml' path='IDirectoryIO/Methods/Instance[@name="EnumerateDirectories[EntryPath,EnumerationOptions]"]/*' />
    IEnumerable<EntryPath> EnumerateDirectories(EntryPath path, EnumerationOptions enumerationOptions);
}
