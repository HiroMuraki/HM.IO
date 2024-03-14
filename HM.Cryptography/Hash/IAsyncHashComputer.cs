namespace HM.Cryptography.Hash;

public interface IAsyncHashComputer
{
    Task<Byte[]> ComputeHashAsync(Byte[] data);
}