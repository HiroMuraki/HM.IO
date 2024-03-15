using HM.Cryptography.Hash;

namespace HM.IO.Previews;

public interface IFileHashComputer
{
    Hash ComputeHash(IFileEntry file);
}
