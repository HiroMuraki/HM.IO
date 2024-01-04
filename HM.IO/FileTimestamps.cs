namespace HM.IO;

/// <include file='Docs/FileTimestamps.xml' path='FileTimestamps/Class[@name="FileTimestamps"]/*' />
public struct FileTimestamps
{
    /// <include file='Docs/FileTimestamps.xml' path='FileTimestamps/Properties/Instance[@name="CreationTime"]/*' />
    public DateTime CreationTime { get; set; }

    /// <include file='Docs/FileTimestamps.xml' path='FileTimestamps/Properties/Instance[@name="LastWriteTime"]/*' />
    public DateTime LastWriteTime { get; set; }

    /// <include file='Docs/FileTimestamps.xml' path='FileTimestamps/Properties/Instance[@name="LastAccessTime"]/*' />
    public DateTime LastAccessTime { get; set; }
}