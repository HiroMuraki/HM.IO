using HM.Cryptography.Hash;

namespace HM.IO.Previews;

public interface IAsyncFileHashComputer
{
    Task<Hash> ComputeHashAsync(IFileEntry file);
}
