namespace HM.Cryptography.Encryptors.Xor;

public abstract class XorCryptographerBase : CryptographerBase
{
    internal XorCryptographerBase(Key key) : base(key)
    {
    }

    #region NonPublic
    protected void XOREncryptCore(Byte[] bytes)
    {
        XOREncryptCore(bytes.AsSpan());
    }
    protected void XOREncryptCore(Span<Byte> bytes)
    {
        for (Int32 i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (Byte)(bytes[i] ^ Key[i % Key.Length]);
        }
    }
    protected ReadOnlySpan<Byte> XOREncryptCore(ReadOnlySpan<Byte> bytes)
    {
        var result = new Span<Byte>(new Byte[bytes.Length]);
        bytes.CopyTo(result);
        for (Int32 i = 0; i < bytes.Length; i++)
        {
            result[i] = (Byte)(bytes[i] ^ Key[i % Key.Length]);
        }
        return result;
    }
    #endregion
}
