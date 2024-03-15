namespace HM.IO.Previews;

public interface IStream :
    IDisposable
{
    StreamMode Mode { get; }

    Int64 SizeInBytes { get; }

    Int32 Read(Byte[] buffer, Int32 offset, Int32 count);

    void Write(Byte[] buffer, Int32 offset, Int32 count);
}
