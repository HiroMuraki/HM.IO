using System.Security.Cryptography;

namespace HM.Cryptography.Hash;

public sealed class MD5Computer :
    HashAlgorithmBasedComputer,
    IHashComputer,
    IAsyncHashComputer
{
    #region NonPublic
    protected override HashAlgorithm GetHashAlgorithm()
    {
        return MD5.Create();
    }
    #endregion
}