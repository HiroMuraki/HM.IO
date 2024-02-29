namespace HM.IO.Previews.Stream;

public interface IAsyncStream :
    IDisposable
{
    StreamMode Mode { get; }

    Int64 SizeInBytes { get; }

    Task<Int32> ReadAsync(Byte[] buffer, Int32 offset, Int32 count);

    Task WriteAsync(Byte[] buffer, Int32 offset, Int32 count);
}