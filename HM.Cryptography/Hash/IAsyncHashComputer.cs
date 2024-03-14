namespace HM.Cryptography.Hash;

public interface IAsyncHashComputer
{
    Task<Hash> ComputeHashAsync(Byte[] data);
}