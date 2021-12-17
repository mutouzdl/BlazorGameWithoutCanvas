using System.Drawing;
using System.Numerics;

namespace AntDesignGameFramework;

/// <summary>
/// 参考来源：https://github.com/mizrael/BlazorCanvas
/// </summary>
public class GameObject : Object
{
    /// <summary>
    /// Web组件类型
    /// </summary>
    public Type WebComponentType { get; }

    private Dictionary<string, object> _params;
    /// <summary>
    /// 提供给Web组件的参数
    /// </summary>
    public Dictionary<string, object> WebParameters
    {
        get
        {
            if (_params == null)
            {
                _params = new Dictionary<string, object>() {
                    { "GameObject", this }
                };
            }

            return _params;
        }
    }

    private static Size _defaultSize = new Size(10, 10);
    private ComponentsCollection Components { get; } = new ComponentsCollection();


    public GameObject(Type webComponentType)
    {
        WebComponentType = webComponentType;
    }

    public async ValueTask Update(GameContext game)
    {
        foreach (var component in this.Components)
        {
            await component.Update(game);
        }

        var childCount = Transform.GetChildCount();
        for (int i = 0; i < childCount; i++)
        {
            var child = Transform.GetChild(i);
            await child.Update(game);
        }

        await OnUpdate(game);
    }

    public void AddComponent(IComponent component)
    {
        component.SetOwner(this);
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

            AddComponent(component);
        }

        return component;
    }

    protected virtual async ValueTask OnUpdate(GameContext game) { }

    private Transform _transform = null;
    public Transform Transform
    {
        get
        {
            if (_transform == null)
            {
                _transform = new Transform(this)
                {
                    Position = Vector2.Zero,
                    Direction = Vector2.One,
                    Size = _defaultSize
                };
                AddComponent(_transform);
            }

            return _transform;
        }
        set
        {
            _transform = value;
        }
    }
}