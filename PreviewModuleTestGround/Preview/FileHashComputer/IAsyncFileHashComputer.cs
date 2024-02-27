namespace HM.IO.Previews;

public interface IAsyncFileHashComputer
{
    Task<FileHash> ComputeHashAsync(IFile file);
}
