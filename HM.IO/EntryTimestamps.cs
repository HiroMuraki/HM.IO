namespace HM.IO;

/// <summary>
/// 	Represents the creation, last write, and last access timestamps of a file.
/// </summary>
public struct EntryTimestamps
{
    /// <summary>
    /// 	Gets or sets the timestamp when the file was created.
    /// </summary>
    /// <returns>
    /// 	A <see cref="DateTime"/> object representing the creation time.
    /// </returns>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 	Gets or sets the timestamp when the file was last written to.
    /// </summary>
    /// <returns>
    /// 	A <see cref="DateTime"/> object representing the last write time.
    /// </returns>
    public DateTime LastWriteTime { get; set; }

    /// <summary>
    /// 	Gets or sets the timestamp when the file was last accessed.
    /// </summary>
    /// <returns>
    /// 	A <see cref="DateTime"/> object representing the last access time.
    /// </returns>
    public DateTime LastAccessTime { get; set; }
}