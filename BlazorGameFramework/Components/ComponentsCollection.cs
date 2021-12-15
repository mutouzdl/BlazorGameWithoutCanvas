﻿using System.Collections;

namespace BlazorGameFramework;

/// <summary>
/// 参考来源：https://github.com/mizrael/BlazorCanvas
/// </summary>
public class ComponentsCollection : IEnumerable<IComponent>
{
    private readonly IDictionary<Type, IComponent> _items;

    public ComponentsCollection()
    {
        _items = new Dictionary<Type, IComponent>();
    }

    public bool Add(IComponent component)
    {
        var type = component.GetType();
        if (_items.ContainsKey(type))
            return false;

        _items.Add(type, component);
        return true;
    }

    public T Get<T>() where T : class, IComponent
    {
        var type = typeof(T);
        return _items.ContainsKey(type) ? _items[type] as T : default;
    }

    public IEnumerator<IComponent> GetEnumerator() => _items.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}