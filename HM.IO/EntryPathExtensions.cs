namespace HM.IO;

public static class EntryPathExtensions
{
    #region Properties
    public static IDirectoryIO DirectoryIO { get; set; } = new DirectoryIO();

    public static IFileIO FileIO { get; set; } = new FileIO();
    #endregion

    #region Methods
    public static EntryType GetEntryType(this ref EntryPath self)
    {
        if (FileIO.Exists(self))
        {
            return EntryType.File;
        }
        else if (DirectoryIO.Exists(self))
        {
            return EntryType.Directory;
        }
        else
        {
            return EntryType.Unknow;
        }
    }

    public static FileInfo AsFileInfo(this ref EntryPath self)
    {
        String filePath = self.StringPath;

        if (!FileIO.Exists(self))
        {
            throw new FileNotFoundException($"Can't open file `{filePath}` because file not exists.");
        }

        return new FileInfo(filePath);
    }

    public static DirectoryInfo AsDirectoryInfo(this ref EntryPath self)
    {
        String directoryPath = self.StringPath;

        if (!DirectoryIO.Exists(self))
        {
            throw new FileNotFoundException($"Can't open directory `{directoryPath}` because directory not exists.");
        }

        return new DirectoryInfo(directoryPath);
    }
    #endregion
}
