namespace HM.IO.Previews;

public static class MemoryDisk
{
    public static MemoryFile CreateFile(FilePath path)
        => CreateFile(path, []);

    public static MemoryFile CreateFile(FilePath path, Byte[] data)
    {
        if (s_files.ContainsKey(path))
        {
            throw new IOException($"`{path.StringPath}` already exists");
        }

        var file = MemoryFile.Create(path, data);
        s_files[path] = file;
        return file;
    }

    public static MemoryFile Get(FilePath path)
    {
        if (s_files.TryGetValue(path, out MemoryFile? file))
        {
            return file;
        }
        else
        {
            throw new IOException($"Unable to find file `{path.StringPath}`");
        }
    }

    public static void ChangeFile(FilePath path, MemoryFile file)
    {
        if (!s_files.ContainsKey(path))
        {
            throw new InvalidOperationException($"The original file `{path.StringPath}` does not exist");
        }

        s_files[path] = file;
    }

    public static void DeleteFile(FilePath path)
    {
        s_files.Remove(path);
    }

    public static void Clear()
    {
        foreach (KeyValuePair<FilePath, MemoryFile> item in s_files)
        {
            item.Value.Delete();
        }

        s_files.Clear();
    }

    #region NonPublic
    private static readonly Dictionary<FilePath, MemoryFile> s_files = [];
    #endregion
}
