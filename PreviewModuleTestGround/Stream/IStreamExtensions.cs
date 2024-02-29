using BclStream = System.IO.Stream;

namespace HM.IO.Previews.Stream;

public static class IStreamExtensions
{
    public static Int32 Read(this IStream self, Byte[] buffer)
    {
        return self.Read(buffer, 0, buffer.Length);
    }

    public static void Write(this IStream self, Byte[] buffer)
    {
        self.Write(buffer, 0, buffer.Length);
    }

    public static async Task CopyToAsync(this IStream self, IStream destinationStream)
        => await self.CopyToAsync(destinationStream, CancellationToken.None);

    public static async Task CopyToAsync(this IStream self, IStream destinationStream, CancellationToken cancellationToken)
    {
        await self.GetBclStream().CopyToAsync(destinationStream.GetBclStream(), cancellationToken);
    }

    public static BclStream GetBclStream(this IStream self)
    {
        if (self is BclStream stream)
        {
            return stream;
        }
        else
        {
            return new ProxyStream(self);
        }
    }

    #region NonPublic
    private sealed class ProxyStream : BclStream
    {
        public override Boolean CanRead => _stream.Mode is StreamMode.ReadOnly or StreamMode.ReadWrite;

        public override Boolean CanSeek => throw new NotSupportedException();

        public override Boolean CanWrite => _stream.Mode is StreamMode.WriteOnly or StreamMode.ReadWrite;

        public override Int64 Length => _stream.SizeInBytes;

        public override Int64 Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public override void Flush() => throw new NotSupportedException();

        public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public override Int64 Seek(Int64 offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(Int64 value) => throw new NotSupportedException();

        public override void Write(Byte[] buffer, Int32 offset, Int32 count)
        {
            _stream.Write(buffer, offset, count);
        }

        public ProxyStream(IStream stream)
        {
            _stream = stream;
        }

        #region NonPublic
        private Boolean _isDisposed;
        private IStream _stream;
        protected override void Dispose(Boolean disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                _stream = null!;
            }

            _isDisposed = true;
        }
        #endregion
    }
    #endregion
}