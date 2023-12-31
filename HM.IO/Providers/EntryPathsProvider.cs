namespace HM.IO.Providers;

/// <summary>
/// Provides an abstract base class for generating entry paths and implements the <see cref="IItemsProvider{T}"/> interface.
/// </summary>
public abstract class EntryPathsProvider :
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

    #region NonPublic
    /// <summary>
    /// Helper method for adding an option to a list and updating the reference to the selected item.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to which the option is added.</param>
    /// <param name="item">The item to be added to the list and the reference to be updated.</param>
    /// <returns>The updated <see cref="EntryPathsProvider"/> instance.</returns>
    protected EntryPathsProvider AddOptionHelper<T>(List<T> list, ref T item)
    {
        if (!list.Contains(item))
        {
            list.Add(item);
        }

        return this;
    }
    #endregion
}
