namespace HM.Cryptography;

public interface IAsyncHashComputer
{
    Task<Hash> ComputeHashAsync(Byte[] data);
}