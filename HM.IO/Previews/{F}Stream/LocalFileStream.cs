namespace HM.IO.Previews;

public sealed class LocalFileStream :
    StreamBase
{
    public override Int64 SizeInBytes => _fileStream.Length;

    public override Boolean CanSeek => _fileStream.CanSeek;

    public override Int64 Position
    {
        get => _fileStream.Position;
        set => _fileStream.Position = value;
    }

    public override Int64 Seek(Int64 offset, SeekOrigin origin)
    {
        return _fileStream.Seek(offset, origin);
    }

    public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
    {
        return _fileStream.Read(buffer, offset, count);
    }

    public override void Write(Byte[] buffer, Int32 offset, Int32 count)
    {
        _fileStream.Write(buffer, offset, count);
    }

    public override void SetLength(Int64 value)
    {
        _fileStream.SetLength(value);
    }

    public override void Flush()
    {
        _fileStream.Flush();
    }

    public LocalFileStream(FilePath filePath, StreamMode mode) : base(mode)
    {
        switch (Mode)
        {
            case StreamMode.ReadOnly:
                _fileStream = new FileStream(filePath.StringPath, FileMode.Open, FileAccess.Read);
                break;
            case StreamMode.WriteOnly:
                _fileStream = new FileStream(filePath.StringPath, FileMode.OpenOrCreate, FileAccess.Write);
                break;
            case StreamMode.ReadWrite:
                _fileStream = new FileStream(filePath.StringPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                break;
            default:
                throw new ArgumentException($"`{mode}` is not a valid value.");
        }
    }

    #region NonPublic
    private Boolean _isDisposed;
    private FileStream _fileStream;
    protected override void Dispose(Boolean disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            _fileStream.Dispose();
            _fileStream = null!;
        }

        _isDisposed = true;
    }
    #endregion
}
