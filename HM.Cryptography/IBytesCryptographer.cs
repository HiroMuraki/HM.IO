﻿namespace HM.Cryptographers;

public interface IBytesCryptographer
{
    ReadOnlySpan<Byte> Encrypt(ReadOnlySpan<Byte> bytes);

    ReadOnlySpan<Byte> Decrypt(ReadOnlySpan<Byte> bytes);
}
