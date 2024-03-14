﻿using System.Security.Cryptography;

namespace HM.Cryptography.Hash;

public sealed class SHA512Computer :
    HashAlgorithmBasedComputer,
    IHashComputer,
    IAsyncHashComputer
{
    #region NonPublic
    protected override HashAlgorithm GetHashAlgorithm()
    {
        return SHA256.Create();
    }
    #endregion
}