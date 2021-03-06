using System.Collections.Concurrent;
using System.Drawing;
using System.Numerics;
using Microsoft.JSInterop;

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
    public string Tag { get; set; }

    public bool IsDestroy { get; private set; } = false;

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

    private ConcurrentQueue<Action> _animationCallbacks = new();

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

        // 动画播放状态回调
        lock (_animationCallbacks)
        {
            while (_animationCallbacks.Count > 0)
            {
                if (_animationCallbacks.TryDequeue(out var action))
                {
                    action();
                }
            }
        }

        await OnUpdate(game);
    }

    public void Destroy()
    {
        IsDestroy = true;

        OnDestroy();
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

    /// <summary>
    /// 获取所有同类型的组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="recursion">是否递归子对象的组件</param>
    /// <returns></returns>
    public T[] GetAllComponents<T>(bool recursion = false) where T : class, IComponent
    {
        var result = new List<T>();
        var component = GetComponent<T>();

        if (component != null)
        {
            result.Add(component);
        }

        if (recursion)
        {
            var childResult = GetAllChildComponents<T>(Transform, recursion);
            if (childResult.Count > 0)
            {
                result.AddRange(childResult);
            }
        }

        return result.ToArray();
    }

    private List<T> GetAllChildComponents<T>(Transform current, bool recursion = false) where T : class, IComponent
    {
        var result = new List<T>();

        for (int i = 0; i < current.GetChildCount(); i++)
        {
            var child = current.GetChild(i);
            var component = child.GetComponent<T>();

            if (component != null)
            {
                result.Add(component);
            }

            if (recursion && child.Transform.GetChildCount() > 0)
            {
                var childResult = GetAllChildComponents<T>(child.Transform, recursion);
                if (childResult.Count > 0)
                {
                    result.AddRange(childResult);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 碰撞开始
    /// </summary>
    /// <param name="collision"></param>
    internal protected virtual void OnCollisionEnter(Collision collision)
    {
    }
    /// <summary>
    /// 碰撞结束
    /// </summary>
    /// <param name="collision"></param>
    internal protected virtual void OnCollisionExit(Collision collision)
    {
    }
    /// <summary>
    /// 碰撞中
    /// </summary>
    /// <param name="collision"></param>
    internal protected virtual void OnCollisionStay(Collision collision)
    {
    }

    [JSInvokable]
    public void AnimationStart(JSAnimationEvent e)
    {
        lock (_animationCallbacks)
        {
            _animationCallbacks.Enqueue(() => { OnAnimationStart(e); });
        }
    }

    [JSInvokable]
    public void AnimationIteration(JSAnimationEvent e)
    {
        lock (_animationCallbacks)
        {
            _animationCallbacks.Enqueue(() => { OnAnimationIteration(e); });
        }
    }

    [JSInvokable]
    public void AnimationEnd(JSAnimationEvent e)
    {
        lock (_animationCallbacks)
        {
            _animationCallbacks.Enqueue(() => { OnAnimationEnd(e); });
        }
    }

    protected virtual void OnDestroy() { }
    protected virtual void OnAnimationStart(JSAnimationEvent e) { }
    protected virtual void OnAnimationIteration(JSAnimationEvent e) { }
    protected virtual void OnAnimationEnd(JSAnimationEvent e) { }
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