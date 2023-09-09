namespace HM.IO;

public interface IFileHashComputer
{
    Task<String> ComputeHashAsync(String filePath);
}
