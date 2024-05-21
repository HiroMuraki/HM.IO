using System.Collections;

namespace HM.AppComponents;

public sealed class AppComponents :
    IEnumerable<IAppComponent>
{
    public TComponent[] GetComponents<TComponent>()
        where TComponent : class, IAppComponent
    {
        return GetComponents(typeof(TComponent)).Cast<TComponent>().ToArray();
    }

    public void AddComponent<TComponent>(TComponent component)
        where TComponent : class, IAppComponent
    {
        _components.Add(component);
    }

    public void RemoveComponent<TComponent>(TComponent component)
        where TComponent : class, IAppComponent
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
    private IEnumerable<IAppComponent> GetComponents(Type componentType)
    {
        foreach (IAppComponent component in _components)
        {
            if (component.GetType() == componentType || component.GetType().IsAssignableTo(componentType))
            {
                yield return component;
            }
        }
    }
    #endregion
}
