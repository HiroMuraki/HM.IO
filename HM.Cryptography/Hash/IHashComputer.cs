namespace HM.Cryptography.Hash;

public interface IHashComputer
{
    Byte[] ComputeHash(Byte[] data);
}
