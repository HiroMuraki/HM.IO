#if OLD2
namespace HM.IO;

public interface IFileHashComputer
{
    Task<String> ComputeHashAsync(EntryPath filePath);
}
#endif