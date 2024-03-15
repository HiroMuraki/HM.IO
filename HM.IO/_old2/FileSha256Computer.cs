#if OLD2
using System.Security.Cryptography;

namespace HM.IO;

public sealed class FileSHA256Computer :
    IFileHashComputer
{
    public async Task<String> ComputeHashAsync(EntryPath filePath)
    {
        using var sha256 = SHA256.Create();
        using FileStream fs = File.OpenRead(filePath.StringPath);
        return Convert.ToHexString(await sha256.ComputeHashAsync(fs));
    }
}
#endif