namespace HM.Cryptography.Encryptors.Xor;

public class XorBytesCryptographer : XorCryptographerBase, IBytesCryptographer
{
    public ReadOnlySpan<Byte> Encrypt(ReadOnlySpan<Byte> bytes)
    {
        return XOREncryptCore(bytes);
    }

    public ReadOnlySpan<Byte> Decrypt(ReadOnlySpan<Byte> bytes)
    {
        return XOREncryptCore(bytes);
    }

    public XorBytesCryptographer(Key key) : base(key) { }
}
