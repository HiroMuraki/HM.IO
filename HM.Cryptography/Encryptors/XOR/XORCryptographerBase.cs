namespace HM.Cryptography.Encryptors.XOR;

public abstract class XORCryptographerBase : CryptographerBase
{
    internal XORCryptographerBase(Byte[] key)
    {
        Key = key;
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
            bytes[i] = (Byte)(bytes[i] ^ _originKey[i % _originKey.Length]);
        }
    }

    protected ReadOnlySpan<Byte> XOREncryptCore(ReadOnlySpan<Byte> bytes)
    {
        var result = new Span<Byte>(new Byte[bytes.Length]);
        bytes.CopyTo(result);
        for (Int32 i = 0; i < bytes.Length; i++)
        {
            result[i] = (Byte)(bytes[i] ^ _originKey[i % _originKey.Length]);
        }
        return result;
    }
    #endregion
}
