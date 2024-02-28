namespace HM.IO;

/// <summary>
/// 	Represents the types of file system entries.
/// </summary>
public enum EntryType
{
    /// <summary>
    /// 	The entry type is unknown or undefined.
    /// </summary>
    Unknow,

    /// <summary>
    /// 	The entry is a file.
    /// </summary>
    File,

    /// <summary>
    /// 	The entry is a directory.
    /// </summary>
    Directory,
}
