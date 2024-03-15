namespace HM.IO.Previews;

public interface IFileEntry
{
    FilePath Path { get; }

    Boolean Exists { get; }

    Int64 SizeInBytes { get; }

    IStream Open(StreamMode mode);

    void Create();

    void Delete();
}
