using HM.Cryptography.Hash;
using HM.IO.Previews.Stream;
using System.Security.Cryptography;

namespace HM.IO.Previews.FileHashComputer;

public abstract class FileHashComputerBase :
    IFileHashComputer, 
    IAsyncFileHashComputer
{
    public Hash ComputeHash(IFileEntry file)
    {
        using HashAlgorithm hashAlgorithm = GetHashAlgorithm();
        using IStream fs = file.Open(StreamMode.ReadOnly);
        Byte[] fileHash = hashAlgorithm.ComputeHash(fs.GetBclStream());
        return new Hash(fileHash);
    }

    public async Task<Hash> ComputeHashAsync(IFileEntry file)
    {
        using HashAlgorithm hashAlgorithm = GetHashAlgorithm();
        using IStream fs = file.Open(StreamMode.ReadOnly);
        Byte[] fileHash = await hashAlgorithm.ComputeHashAsync(fs.GetBclStream());
        return new Hash(fileHash);
    }

    #region NonPublic
    protected abstract HashAlgorithm GetHashAlgorithm();
    #endregion
}

