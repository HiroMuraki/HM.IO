using HM.IO.Previews.Stream;

namespace HM.IO.Previews.File;

public sealed class MemoryFile :
    IFile
{
    public FilePath Path => _filePath;

    public Int64 SizeInBytes => _data.Length;

    public Boolean Exists => !_isDeleted;

    public EntryTimestamps Timestamps
    {
        get => _timestamps;
        set => _timestamps = value;
    }

    public EntryAttributes Attributes
    {
        get => _attributes;
        set => _attributes = value;
    }

    public void Create()
    {
        if (_isDeleted)
        {
            _data = [];
            _isDeleted = false;
            MemoryDisk.CreateFile(Path);
            MemoryDisk.ChangeFile(Path, this);
        }
    }

    public void Delete()
    {
        if (_isDeleted)
        {
            return;
        }

        MemoryDisk.DeleteFile(Path);

        _data = [];
        _isDeleted = true;
    }

    public IStream Open(StreamMode mode)
    {
        return new MemoryFileStream(this, mode);
    }

    internal static MemoryFile Create(FilePath filePath, Byte[] data)
    {
        return new MemoryFile(filePath, data);
    }

    internal void SetSourceDataHandle(Byte[] data)
    {
        _data = data;
    }

    internal Byte[] GetSourceDataHandle()
    {
        return _data;
    }

    #region NonPublic
    private readonly FilePath _filePath;
    private Byte[] _data;
    private Boolean _isDeleted;
    private EntryTimestamps _timestamps;
    private EntryAttributes _attributes;
    private MemoryFile(FilePath filePath, Byte[] data)
    {
        _filePath = filePath;
        _data = data;
    }
    #endregion
}
