using HM.Cryptography.Hash;
using HM.IO.Previews.Entry;

namespace HM.IO.Previews.FileHashComputer;

public interface IFileHashComputer
{
    Hash ComputeHash(IFile file);
}
