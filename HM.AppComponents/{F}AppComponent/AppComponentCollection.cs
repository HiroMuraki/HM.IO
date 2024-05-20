using System.Collections;

namespace HM.AppComponents;

public sealed class AppComponentCollection :
    IEnumerable<IAppComponent>
{
    public Boolean Contains(IAppComponent component)
    {
        return _components.Contains(component);
    }

    public void Add(IAppComponent component)
    {
        _components.Add(component);
    }

    public void Remove(IAppComponent component)
    {
        _components.Remove(component);
    }

    public IEnumerator<IAppComponent> GetEnumerator()
    {
        foreach (IAppComponent component in _components)
        {
            yield return component;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #region NonPublic
    private readonly List<IAppComponent> _components = [];
    #endregion
}