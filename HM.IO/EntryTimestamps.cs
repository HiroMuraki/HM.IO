namespace HM.IO;

/// <include file='EntryTimestamps.xml' path='FileTimestamps/Class[@name="EntryTimestamps"]/*' />
public struct EntryTimestamps
{
    /// <include file='EntryTimestamps.xml' path='FileTimestamps/Properties/Instance[@name="CreationTime"]/*' />
    public DateTime CreationTime { get; set; }

    /// <include file='EntryTimestamps.xml' path='FileTimestamps/Properties/Instance[@name="LastWriteTime"]/*' />
    public DateTime LastWriteTime { get; set; }

    /// <include file='EntryTimestamps.xml' path='FileTimestamps/Properties/Instance[@name="LastAccessTime"]/*' />
    public DateTime LastAccessTime { get; set; }
}