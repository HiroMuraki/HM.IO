using System.Security.Cryptography;

namespace HM.Cryptography.Hash;

public abstract class HashAlgorithmBasedComputer :
    IHashComputer,
    IAsyncHashComputer
{
    public Byte[] ComputeHash(Byte[] data)
    {
        using HashAlgorithm hashAlgorithm = GetHashAlgorithm();
        using var memoryStream = new MemoryStream(data, writable: false);
        return hashAlgorithm.ComputeHash(memoryStream);
    }

    public async Task<Byte[]> ComputeHashAsync(Byte[] data)
    {
        using HashAlgorithm hashAlgorithm = GetHashAlgorithm();
        using var memoryStream = new MemoryStream(data, writable: false);
        return await hashAlgorithm.ComputeHashAsync(memoryStream);
    }

    #region NonPublic
    protected abstract HashAlgorithm GetHashAlgorithm();
    #endregion
}
