using HM.IO.Previews.Entry;

namespace HM.IO.Previews.FileHashComputer;

public interface IFileHashComputer
{
    FileHash ComputeHash(IFile file);
}
