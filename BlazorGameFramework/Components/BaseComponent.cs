namespace BlazorGameFramework;

/// <summary>
/// 参考来源：https://github.com/mizrael/BlazorCanvas
/// </summary>
public abstract class BaseComponent : IComponent
{
    public string Uid { get; } = Guid.NewGuid().ToString();

    public GameObject Owner { get; }

    protected BaseComponent(GameObject owner)
    {
        this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        this.Owner.AddComponent(this);
    }

    public virtual async ValueTask Update(GameContext game)
    {
    }
}
