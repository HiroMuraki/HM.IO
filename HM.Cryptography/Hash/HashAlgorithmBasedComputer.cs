using System.Security.Cryptography;

namespace HM.Cryptography.Hash;

public abstract class HashAlgorithmBasedComputer :
    IHashComputer,
    IAsyncHashComputer
{
    public Hash ComputeHash(Byte[] data)
    {
        using HashAlgorithm hashAlgorithm = GetHashAlgorithm();
        using var memoryStream = new MemoryStream(data, writable: false);
        return new Hash(hashAlgorithm.ComputeHash(memoryStream));
    }

    public async Task<Hash> ComputeHashAsync(Byte[] data)
    {
        using HashAlgorithm hashAlgorithm = GetHashAlgorithm();
        using var memoryStream = new MemoryStream(data, writable: false);
        return new Hash(await hashAlgorithm.ComputeHashAsync(memoryStream));
    }

    #region NonPublic
    protected abstract HashAlgorithm GetHashAlgorithm();
    #endregion
}
