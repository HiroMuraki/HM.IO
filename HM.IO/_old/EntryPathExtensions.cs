//namespace HM.IO;

///// <summary>
///// 	Provides extension methods for working with entry paths, such as directories and files.
///// </summary>
//public static class EntryPathExtensions
//{
//    #region Properties
//    /// <summary>
//    /// 	Gets the <see cref="IDirectoryIO"/> instance associated with the directory specified by the entry path.
//    /// </summary>
//    /// <value>
//    /// 	The <see cref="IDirectoryIO"/> instance.
//    /// </value>
//    public static IDirectoryIO DirectoryIO { get; set; } = IO.DirectoryIO.Default;

//    /// <summary>
//    /// 	Gets the <see cref="IFileIO"/> instance associated with the file specified by the entry path.
//    /// </summary>
//    /// <value>
//    /// 	The <see cref="IFileIO"/> instance.
//    /// </value>
//    public static IFileIO FileIO { get; set; } = IO.FileIO.Default;
//    #endregion

//    #region Methods
//    /// <summary>
//    /// 	Gets the type of the entry specified by the entry path (either file or directory).
//    /// </summary>
//    /// <param name="self">The instance of EntryPath.</param>
//    /// <returns>
//    /// 	A value indicating whether the entry is a file, directory, or does not exist.
//    /// </returns>
//    public static EntryType GetEntryType(this ref EntryPath self)
//    {
//        if (FileIO.Exists(self))
//        {
//            return EntryType.File;
//        }
//        else if (DirectoryIO.Exists(self))
//        {
//            return EntryType.Directory;
//        }
//        else
//        {
//            return EntryType.Unknow;
//        }
//    }

//    /// <summary>
//    /// 	Converts the entry path to a <see cref="FileInfo"/> instance, providing file-related information and operations.
//    /// </summary>
//    /// <param name="self">The instance of EntryPath.</param>
//    /// <returns>
//    /// 	A <see cref="FileInfo"/> instance representing the specified file.
//    /// </returns>
//    public static FileInfo ToFileInfo(this ref EntryPath self)
//    {
//        String filePath = self.StringPath;

//        if (!FileIO.Exists(self))
//        {
//            throw new FileNotFoundException($"Can't open file `{filePath}` because file does not exist.");
//        }

//        return new FileInfo(filePath);
//    }

//    /// <summary>
//    /// 	Converts the entry path to a <see cref="DirectoryInfo"/> instance, providing directory-related information and operations.
//    /// </summary>
//    /// <param name="self">The instance of EntryPath.</param>
//    /// <returns>
//    /// 	A <see cref="DirectoryInfo"/> instance representing the specified directory.
//    /// </returns>
//    public static DirectoryInfo ToDirectoryInfo(this ref EntryPath self)
//    {
//        String directoryPath = self.StringPath;

//        if (!DirectoryIO.Exists(self))
//        {
//            throw new FileNotFoundException($"Can't open directory `{directoryPath}` because directory does not exist.");
//        }

//        return new DirectoryInfo(directoryPath);
//    }
//    #endregion
//}
