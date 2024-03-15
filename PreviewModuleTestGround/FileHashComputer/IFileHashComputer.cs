using HM.Cryptography.Hash;

namespace HM.IO.Previews.FileHashComputer;

public interface IFileHashComputer
{
    Hash ComputeHash(IFileEntry file);
}
