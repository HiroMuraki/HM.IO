namespace HM.IO.Previews;

public interface IFileHashComputer
{
    FileHash ComputeHash(IFile file);
}
