namespace HM.IO.Previews;

public interface IFile
{
    FilePath Path { get; }

    Boolean Exists { get; }

    Int64 SizeInBytes { get; }

    EntryTimestamps Timestamps { get; set; }

    EntryAttributes Attributes { get; set; }

    IStream Open(StreamMode mode);

    void Create();

    void Delete();
}
