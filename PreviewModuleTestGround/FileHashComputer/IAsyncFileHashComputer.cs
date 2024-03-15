using HM.Cryptography.Hash;

namespace HM.IO.Previews.FileHashComputer;

public interface IAsyncFileHashComputer
{
    Task<Hash> ComputeHashAsync(IFileEntry file);
}
