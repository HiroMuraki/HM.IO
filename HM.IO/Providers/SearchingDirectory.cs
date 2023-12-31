namespace HM.IO;

/// <summary>
/// Represents a readonly structure for specifying a directory to be searched, including optional parameters.
/// </summary>
public readonly struct SearchingDirectory
{
    /// <summary>
    /// Gets the path of the directory to be searched.
    /// </summary>
    public EntryPath Path { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether to recurse into subdirectories when searching (default is false).
    /// </summary>
    public Boolean RecurseSubdirectories { get; init; } = false;

    /// <summary>
    /// Gets or sets the maximum recursion depth when searching subdirectories (default is Int32.MaxValue).
    /// </summary>
    public Int32 MaxRecursionDepth { get; init; } = Int32.MaxValue;

    /// <summary>
    /// Gets or sets a value indicating whether to ignore the directory if it does not exist (default is false).
    /// </summary>
    public Boolean IgnoreIfNotExists { get; init; } = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchingDirectory"/> structure with the specified directory path.
    /// </summary>
    /// <param name="path">The path of the directory to be searched.</param>
    public SearchingDirectory(EntryPath path)
    {
        Path = path;
    }
}
