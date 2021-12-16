namespace BlazorGameFramework;

/// <summary>
/// 参考来源：https://github.com/mizrael/BlazorCanvas
/// </summary>
public abstract class BaseComponent : Object, IComponent
{
    public GameObject Owner { get; protected set; }

    public BaseComponent()
    {

    }

    protected BaseComponent(GameObject owner)
    {
        this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        this.Owner.AddComponent(this);
    }

    public void SetOwner(GameObject owner)
    {
        Owner = owner;
    }

    public virtual async ValueTask Update(GameContext game) { }
    public virtual async ValueTask Render(GameContext game) { }
}
