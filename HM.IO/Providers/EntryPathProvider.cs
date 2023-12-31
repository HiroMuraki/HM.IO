namespace HM.IO.Providers;

/// <summary>
/// Provides an abstract base class for generating entry paths and implements the <see cref="IItemsProvider{T}"/> interface.
/// </summary>
public abstract class EntryPathProvider :
    IItemsProvider<EntryPath>
{
    /// <summary>
    /// Gets or sets the directory input/output operations for the entry path provider.
    /// </summary>
    public IDirectoryIO DirectoryIO { get; protected set; } = new DirectoryIO();

    /// <summary>
    /// Enumerates and returns a collection of entry paths.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> of entry paths.</returns>
    public abstract IEnumerable<EntryPath> EnumerateItems();
}
