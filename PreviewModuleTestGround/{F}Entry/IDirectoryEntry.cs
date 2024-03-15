namespace HM.IO.Previews;

public interface IDirectoryEntry
{
    DirectoryPath Path { get; }

    Boolean Exists { get; }

    IEnumerable<IFileEntry> EnumerateFiles(EnumerationOptions enumerationOptions);

    IEnumerable<IDirectoryEntry> EnumerateDirectory(EnumerationOptions enumerationOptions);
}
