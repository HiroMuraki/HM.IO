using HM.IO.Previews.File;

namespace HM.IO.Previews.FileHashComputer;

public interface IAsyncFileHashComputer
{
    Task<FileHash> ComputeHashAsync(IFile file);
}
