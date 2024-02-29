using HM.IO.Previews.File;

namespace HM.IO.Previews.Stream;

public sealed class MemoryFileStream :
    StreamBase
{
    public override Int64 SizeInBytes => _memoryFile.GetSourceDataHandle().Length;

    public override Boolean CanSeek => true;

    public override Int64 Position
    {
        get => _position;
        set => _position = value;
    }

    public override Int64 Seek(Int64 offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                _position = offset;
                break;
            case SeekOrigin.Current:
                _position += offset;
                break;
            case SeekOrigin.End:
                _position += offset;
                break;
        }

        return _position;
    }

    public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
    {
        Byte[] sourceData = _memoryFile.GetSourceDataHandle();

        if (_position >= sourceData.Length)
        {
            return 0;
        }

        Int64 forwardDataSize = sourceData.Length - _position;
        Int64 expcetedWriteLength = count < forwardDataSize ? count : forwardDataSize;
        Array.Copy(
            sourceArray: sourceData,
            sourceIndex: _position,
            destinationArray: buffer,
            destinationIndex: offset,
            length: expcetedWriteLength);

        Int32 readCount = count;
        if (forwardDataSize < count)
        {
            readCount = (Int32)forwardDataSize;
        }

        _position += readCount;

        return readCount;
    }

    public override void Write(Byte[] buffer, Int32 offset, Int32 count)
    {
        Byte[] sourceData = _memoryFile.GetSourceDataHandle();
        Int64 startPosition = _position + offset;
        Int64 expectedLength = startPosition + count;

        if (sourceData.Length >= expectedLength)
        {
            buffer.CopyTo(sourceData, startPosition);
        }
        else
        {
            Byte[] newBytes = new Byte[expectedLength];

            sourceData.CopyTo(newBytes, 0);

            for (Int32 i = 0; i < count; i++)
            {
                newBytes[startPosition + i] = buffer[i];
            }

            _memoryFile.SetSourceDataHandle(newBytes);
        }

        _position += count;
    }

    public override void SetLength(Int64 value)
    {
        if (value < _memoryFile.SizeInBytes)
        {
            throw new InvalidOperationException($"Unable to set a smaller value of length for {typeof(MemoryFileStream)}");
        }

        Byte[] newBytes = new Byte[value];
        _memoryFile.GetSourceDataHandle().CopyTo(newBytes, 0);
        _memoryFile.SetSourceDataHandle(newBytes);
    }

    public override void Flush() { }

    internal MemoryFileStream(MemoryFile memoryFile, StreamMode mode) : base(mode)
    {
        _memoryFile = memoryFile;
    }

    #region NonPublic
    private Boolean _isDisposed;
    private MemoryFile _memoryFile;
    private Int64 _position;
    protected override void Dispose(Boolean disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            _memoryFile = null!;
        }

        _isDisposed = true;
    }
    #endregion
}
