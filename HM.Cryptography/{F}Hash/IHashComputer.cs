namespace HM.Cryptography;

public interface IHashComputer
{
    Hash ComputeHash(Byte[] data);
}
