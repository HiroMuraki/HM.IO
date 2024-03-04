namespace HM.Cryptographers;

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

    public AesBytesCryptographer(Byte[] key) : base(key) { }
    public AesBytesCryptographer(Byte[] key, Byte[] iv) : base(key, iv) { }
}
