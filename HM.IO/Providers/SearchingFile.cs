namespace HM.IO;

/// <summary>
/// Represents a readonly structure for specifying a file to be searched, including optional parameters.
/// </summary>
public readonly struct SearchingFile
{
    /// <summary>
    /// Gets the path of the file to be searched.
    /// </summary>
    public EntryPath Path { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether to ignore the file if it does not exist (default is false).
    /// </summary>
    public Boolean IgnoreIfNotExists { get; init; } = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchingFile"/> structure with the specified file path.
    /// </summary>
    /// <param name="path">The path of the file to be searched.</param>
    public SearchingFile(EntryPath path)
    {
        Path = path;
    }
}

public class FileEnumerationOptions
{
    public FileAttributes AttributesToSkip { get; init; } = (FileAttributes)Int32.MinValue;

    public Boolean IgnoreInaccessible { get; init; } = true;

    public Boolean ReturnSpecialDirectories { get; init; } = false;

    public EnumerationOptions ToEnumerationOptions()
    {
        return new EnumerationOptions
        {
            AttributesToSkip = AttributesToSkip,
            IgnoreInaccessible = IgnoreInaccessible,
            ReturnSpecialDirectories = ReturnSpecialDirectories,
        };
    }
}

public class DirectoryEnumerationOptions
{
    public FileAttributes AttributesToSkip { get; init; } = (FileAttributes)Int32.MinValue;

    public Boolean IgnoreInaccessible { get; init; } = true;

    public Boolean ReturnSpecialDirectories { get; init; } = false;

    public EnumerationOptions ToEnumerationOptions()
    {
        return new EnumerationOptions
        {
            AttributesToSkip = AttributesToSkip,
            IgnoreInaccessible = IgnoreInaccessible,
            ReturnSpecialDirectories = ReturnSpecialDirectories,
        };
    }
}