using BclFile = System.IO.File;

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
                CreationTime = BclFile.GetCreationTime(GetStringPath()),
                LastWriteTime = BclFile.GetLastWriteTime(GetStringPath()),
                LastAccessTime = BclFile.GetLastAccessTime(GetStringPath()),
            };
        }
        set
        {
            EntryTimestamps timestamps = value;
            BclFile.SetCreationTime(GetStringPath(), timestamps.CreationTime);
            BclFile.SetLastWriteTime(GetStringPath(), timestamps.LastWriteTime);
            BclFile.SetLastAccessTime(GetStringPath(), timestamps.LastAccessTime);
        }
    }

    public EntryAttributes Attributes
    {
        get => new(BclFile.GetAttributes(GetStringPath()));
        set => BclFile.SetAttributes(GetStringPath(), value.Value);
    }

    #region NonPublic
    protected abstract String GetStringPath();
    #endregion
}
