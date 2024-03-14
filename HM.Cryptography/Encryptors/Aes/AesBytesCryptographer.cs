namespace HM.Cryptography.Encryptors.Aes;

public class AesBytesCryptographer : AesCryptographerBase, IBytesCryptographer
{
    public ReadOnlySpan<Byte> Decrypt(ReadOnlySpan<Byte> bytes)
    {
        return ProcessCore(bytes, _aes.CreateDecryptor());
    }

    public ReadOnlySpan<Byte> Encrypt(ReadOnlySpan<Byte> bytes)
    {
        return ProcessCore(bytes, _aes.CreateEncryptor());
    }

    public AesBytesCryptographer(Key key) : base(key) { }

    public AesBytesCryptographer(Key key, Byte[] iv) : base(key, iv) { }
}
