namespace HM.IO.Previews;

public abstract class LocalEntryBase
{
    public abstract Boolean Exists { get; }

    public EntryTimestamps Timestamps
    {
        get
        {
            return new EntryTimestamps
            {
                CreationTime = File.GetCreationTime(GetStringPath()),
                LastWriteTime = File.GetLastWriteTime(GetStringPath()),
                LastAccessTime = File.GetLastAccessTime(GetStringPath()),
            };
        }
        set
        {
            EntryTimestamps timestamps = value;
            File.SetCreationTime(GetStringPath(), timestamps.CreationTime);
            File.SetLastWriteTime(GetStringPath(), timestamps.LastWriteTime);
            File.SetLastAccessTime(GetStringPath(), timestamps.LastAccessTime);
        }
    }

    public EntryAttributes Attributes
    {
        get => new(File.GetAttributes(GetStringPath()));
        set => File.SetAttributes(GetStringPath(), value.Value);
    }

    #region NonPublic
    protected abstract String GetStringPath();
    #endregion
}
