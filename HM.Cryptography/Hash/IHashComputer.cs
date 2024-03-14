namespace HM.Cryptography.Hash;

public interface IHashComputer
{
    Hash ComputeHash(Byte[] data);
}
