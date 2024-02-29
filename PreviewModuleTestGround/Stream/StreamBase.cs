using BclStream = System.IO.Stream;

namespace HM.IO.Previews.Stream;

public abstract class StreamBase :
    BclStream,
    IStream,
    IAsyncStream
{
    public StreamMode Mode => _mode;

    public abstract Int64 SizeInBytes { get; }

    public sealed override Int64 Length => SizeInBytes;

    public sealed override Boolean CanRead => _mode is StreamMode.ReadOnly or StreamMode.ReadWrite;

    public sealed override Boolean CanWrite => _mode is StreamMode.WriteOnly or StreamMode.ReadWrite;

    #region NonPublic
    private readonly StreamMode _mode;
    protected StreamBase(StreamMode mode)
    {
        _mode = mode;
    }
    #endregion
}
