namespace HM.IO;

public interface IItemsProvider<T>
{
    IEnumerable<T> EnumerateItems();
}
