namespace HM.IO;

/// <summary>
/// Represents a generic interface for providing a collection of items.
/// </summary>
/// <typeparam name="T">The type of items in the collection.</typeparam>
public interface IItemsProvider<T>
{
    /// <summary>
    /// Enumerates through the collection of items.
    /// </summary>
    /// <returns>An IEnumerable of items.</returns>
    IEnumerable<T> EnumerateItems();
}
