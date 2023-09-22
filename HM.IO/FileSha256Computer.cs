using System.Security.Cryptography;

namespace HM.IO;

public sealed class FileSha256Computer : IFileHashComputer
{
    public async Task<String> ComputeHashAsync(String filePath)
    {
        using var sha256 = SHA256.Create();
        using FileStream fs = File.OpenRead(filePath);
        return Convert.ToHexString(await sha256.ComputeHashAsync(fs));
    }
}