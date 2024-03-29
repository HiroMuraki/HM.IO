﻿using System.Security.Cryptography;

namespace HM.IO.Previews;

public sealed class FileSHA256Computer :
    FileHashComputerBase
{
    #region NonPublic
    protected override HashAlgorithm GetHashAlgorithm()
    {
        return SHA256.Create();
    }
    #endregion
}

