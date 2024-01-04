namespace HM.IO;

/// <include file='FileTimestamps.xml' path='FileTimestamps/Class[@name="FileTimestamps"]/*' />
public struct FileTimestamps
{
    /// <include file='FileTimestamps.xml' path='FileTimestamps/Properties/Instance[@name="CreationTime"]/*' />
    public DateTime CreationTime { get; set; }

    /// <include file='FileTimestamps.xml' path='FileTimestamps/Properties/Instance[@name="LastWriteTime"]/*' />
    public DateTime LastWriteTime { get; set; }

    /// <include file='FileTimestamps.xml' path='FileTimestamps/Properties/Instance[@name="LastAccessTime"]/*' />
    public DateTime LastAccessTime { get; set; }
}