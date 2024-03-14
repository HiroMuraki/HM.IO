namespace HM.Cryptography.Encryptors.XOR;

public class XORBytesCryptographer : XORCryptographerBase, IBytesCryptographer
{
    public ReadOnlySpan<Byte> Encrypt(ReadOnlySpan<Byte> bytes)
    {
        return XOREncryptCore(bytes);
    }

    public ReadOnlySpan<Byte> Decrypt(ReadOnlySpan<Byte> bytes)
    {
        return XOREncryptCore(bytes);
    }

    public XORBytesCryptographer(Byte[] key) : base(key) { }
}
