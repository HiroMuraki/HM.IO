using System.Security.Cryptography;

namespace HM.Cryptographers;

public abstract class AesCryptographerBase : CryptographerBase
{
    public static readonly Int32 KeyLength = 256 / 8;
    public static readonly Int32 IVLength = 128 / 8;

    internal AesCryptographerBase(Byte[] key)
    {
        if (key.Length != KeyLength)
        {
            throw new ArgumentException($"Requires a {KeyLength} length key");
        }
        _aes.Key = key;
        _aes.IV = new Byte[IVLength];
    }
    internal AesCryptographerBase(Byte[] key, Byte[] iv) : this(key)
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
    protected readonly Aes _aes = Aes.Create();
    #endregion
}
