﻿using System.Security.Cryptography;

namespace HM.IO.Previews;

public sealed class FileMD5Computer :
    FileHashComputerBase
{
    protected override HashAlgorithm GetHashAlgorithm()
    {
        return MD5.Create();
    }
}

