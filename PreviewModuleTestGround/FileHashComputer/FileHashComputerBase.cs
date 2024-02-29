using HM.IO.Previews.File;
using HM.IO.Previews.Stream;
using System.Security.Cryptography;

namespace HM.IO.Previews.FileHashComputer;

public abstract class FileHashComputerBase : IFileHashComputer, IAsyncFileHashComputer
{
    public FileHash ComputeHash(IFile file)
    {
        using HashAlgorithm hashAlgorithm = GetHashAlgorithm();
        using IStream fs = file.Open(StreamMode.ReadOnly);
        Byte[] fileHash = hashAlgorithm.ComputeHash(fs.GetBclStream());
        return new FileHash(fileHash);
    }

    public async Task<FileHash> ComputeHashAsync(IFile file)
    {
        using HashAlgorithm hashAlgorithm = GetHashAlgorithm();
        using IStream fs = file.Open(StreamMode.ReadOnly);
        Byte[] fileHash = await hashAlgorithm.ComputeHashAsync(fs.GetBclStream());
        return new FileHash(fileHash);
    }

    #region NonPublic
    protected abstract HashAlgorithm GetHashAlgorithm();
    #endregion
}

