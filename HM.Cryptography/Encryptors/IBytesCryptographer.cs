namespace HM.Cryptography.Encryptors;

public interface IBytesCryptographer
{
    ReadOnlySpan<Byte> Encrypt(ReadOnlySpan<Byte> bytes);

    ReadOnlySpan<Byte> Decrypt(ReadOnlySpan<Byte> bytes);
}
