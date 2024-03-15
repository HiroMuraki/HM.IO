using HM.Cryptography;

namespace HM.IO.Previews;

public interface IAsyncFileHashComputer
{
    Task<Hash> ComputeHashAsync(IFileEntry file);
}
