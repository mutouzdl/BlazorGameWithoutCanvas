using System.Drawing;
using System.Numerics;

namespace AntDesignGameFramework;

/// <summary>
/// 参考来源：https://github.com/mizrael/BlazorCanvas
/// </summary>
public class Transform : BaseComponent
{
    private Vector2 _localPosition = Vector2.Zero;
    public Vector2 LocalPosition
    {
        get
        {
            return _localPosition;
        }
        set
        {
            var offsetX = value.X - _localPosition.X;
            var offsetY = value.Y - _localPosition.Y;

            /* 修改本地坐标时，同时更新世界坐标 */
            Position = new Vector2(Position.X + offsetX, Position.Y + offsetY);

            _localPosition = value;

            UpdateChildrenPosition();
        }
    }

    private Vector2 _position = Vector2.Zero;
    public Vector2 Position
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;

            /* 修改坐标时，同时更新子对象坐标 */
            UpdateChildrenPosition();
        }
    }
    public Vector2 Direction { get; set; } = Vector2.UnitX;
    public Size Size { get; set; } = Size.Empty;

    public Transform Parent { get; private set; }

    private List<GameObject> _children = new();

    public Rectangle BoundingBox => new Rectangle()
    {
        Size = Size,
        X = (int)Position.X,
        Y = (int)Position.Y,
    };

    public Transform(GameObject owner) : base(owner)
    {
        if (Parent != null)
        {
            Position = Parent.Position;
        }
    }

    public GameObject GetChild(int index)
    {
        return _children[index];
    }

    public void AddChild(GameObject child)
    {
        if (_children.Any(t => t.Uid == child.Uid))
        {
            return;
        }

        _children.Add(child);

        UpdateChildPosition(child);
    }

    public void SetParent(GameObject parent)
    {
        Parent = parent.Transform;
        parent.Transform.AddChild(this.Owner);
    }

    public int GetChildCount() => _children.Count;

    private void UpdateChildrenPosition()
    {
        foreach (var child in _children)
        {
            UpdateChildPosition(child);
        }
    }

    private void UpdateChildPosition(GameObject child)
    {
        child.Transform.Position = this.Position + child.Transform.LocalPosition;
    }
}
