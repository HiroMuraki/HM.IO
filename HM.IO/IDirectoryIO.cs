namespace HM.IO;

public interface IDirectoryIO
{
    IEnumerable<String> EnumerateFiles(EntryPath path, EnumerationOptions enumerationOptions);

    IEnumerable<String> EnumerateDirectories(EntryPath path, EnumerationOptions enumerationOptions);
}
