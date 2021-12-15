namespace BlazorGameFramework;

/// <summary>
/// 参考来源：https://github.com/mizrael/BlazorCanvas
/// </summary>
public class GameObject
{
    private ComponentsCollection Components { get; } = new ComponentsCollection();

    public async ValueTask Update(GameContext game)
    {
        foreach (var component in this.Components)
        {
            await component.Update(game);
        }
    }

    public void AddComponent(IComponent component)
    {
        Components.Add(component);
    }

    public T GetComponent<T>() where T : class, IComponent
    {
        return Components.Get<T>();
    }

    public T GetOrAddComponent<T>() where T : class, IComponent
    {
        var component = Components.Get<T>();

        if (component == null)
        {
            component = Activator.CreateInstance<T>();

            Components.Add(component);
        }

        return component;
    }
}