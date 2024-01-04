namespace HM.IO;

public static class EntryPathExtensions
{
    #region Properties
    /// <include file='Docs/EntryPathExtensions.xml' path='EntryPathExtensions/Properties/Static[@name="DirectoryIO"]/*' />
    public static IDirectoryIO DirectoryIO { get; set; } = new DirectoryIO();

    /// <include file='Docs/EntryPathExtensions.xml' path='EntryPathExtensions/Properties/Static[@name="FileIO"]/*' />
    public static IFileIO FileIO { get; set; } = new FileIO();
    #endregion

    #region Methods
    /// <include file='Docs/EntryPathExtensions.xml' path='EntryPathExtensions/Methods/Static[@name="GetEntryType[EntryPath]"]/*' />
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

    /// <include file='Docs/EntryPathExtensions.xml' path='EntryPathExtensions/Methods/Static[@name="ToFileInfo[EntryPath]"]/*' />
    public static FileInfo ToFileInfo(this ref EntryPath self)
    {
        String filePath = self.StringPath;

        if (!FileIO.Exists(self))
        {
            throw new FileNotFoundException($"Can't open file `{filePath}` because file not exists.");
        }

        return new FileInfo(filePath);
    }

    /// <include file='Docs/EntryPathExtensions.xml' path='EntryPathExtensions/Methods/Static[@name="ToDirectoryInfo[EntryPath]"]/*' />
    public static DirectoryInfo ToDirectoryInfo(this ref EntryPath self)
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
