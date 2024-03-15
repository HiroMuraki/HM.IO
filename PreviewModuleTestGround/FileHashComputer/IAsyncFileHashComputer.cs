using HM.Cryptography.Hash;
using HM.IO.Previews.Entry;

namespace HM.IO.Previews.FileHashComputer;

public interface IAsyncFileHashComputer
{
    Task<Hash> ComputeHashAsync(IFile file);
}
