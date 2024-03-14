using System.Security.Cryptography;

namespace HM.Cryptography.Encryptors.Aes;

public abstract class AesCryptographerBase : CryptographerBase
{
    public static readonly Int32 KeyLength = 256 / 8;

    public static readonly Int32 IVLength = 128 / 8;

    internal AesCryptographerBase(Key key) : base(key)
    {
        if (key.Length != KeyLength)
        {
            throw new ArgumentException($"Requires a {KeyLength} length key");
        }
        _aes.Key = key.BinaryValue;
        _aes.IV = new Byte[IVLength];
    }

    internal AesCryptographerBase(Key key, Byte[] iv) : this(key)
    {
        _aes.IV = iv;
    }

    #region NonPublic
    protected static Byte[] ProcessCore(ReadOnlySpan<Byte> buffer, ICryptoTransform transform)
    {
        Byte[] output;
        using (var memoryStream = new MemoryStream())
        {
            using (var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
            {
                using (var writer = new BufferedStream(cryptoStream))
                {
                    writer.Write(buffer);
                }
                output = memoryStream.ToArray();
            }
        }
        return output;
    }
    protected static Byte[] ProcessCore(Byte[] buffer, ICryptoTransform transform)
    {
        return ProcessCore(buffer.AsSpan(), transform);
    }
    protected readonly System.Security.Cryptography.Aes _aes = System.Security.Cryptography.Aes.Create();
    #endregion
}
