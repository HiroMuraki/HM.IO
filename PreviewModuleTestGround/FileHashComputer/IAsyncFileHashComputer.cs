using HM.IO.Previews.Entry;

namespace HM.IO.Previews.FileHashComputer;

public interface IAsyncFileHashComputer
{
    Task<FileHash> ComputeHashAsync(IFile file);
}
