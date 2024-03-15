using HM.Cryptography;

namespace HM.IO.Previews;

public interface IFileHashComputer
{
    Hash ComputeHash(IFileEntry file);
}
