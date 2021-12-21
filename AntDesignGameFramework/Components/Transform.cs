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
    /// <summary>
    /// 中心点
    /// </summary>
    public Vector2 Pivot { get; set; } = new Vector2(0.5f, 0.5f);
    /// <summary>
    /// 参考Unity的RectTransform的AnchorMin实现，但坐标系的零点和终点为左上角和右下角
    /// The normalized position in the parent Transform that the up left corner is anchored to.
    /// </summary>
    public Vector2 AnchorMin { get; set; } = new Vector2(0.5f, 0.5f);
    /// <summary>
    /// 参考Unity的RectTransform的AnchorMax实现，但坐标系的零点和终点为左上角和右下角
    /// The normalized position in the parent Transform that the bottom right corner is anchored to.
    /// </summary>
    public Vector2 AnchorMax { get; set; } = new Vector2(0.5f, 0.5f);

    /// <summary>
    /// 对应left、top
    /// </summary>
    public Vector2 OffsetMin { get; set; } = Vector2.Zero;
    /// <summary>
    /// 对应right、bottom
    /// </summary>
    public Vector2 OffsetMax { get; set; } = Vector2.Zero;
    public Size Size { get; set; } = Size.Empty;

    public Transform Parent { get; private set; }
    public RectTransform Rect { get; private set; }

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

    public override async ValueTask Update(GameContext game)
    {
        _children.RemoveAll(t => t.IsDestroy);
    }

    public void AddChild(GameObject child)
    {
        if (_children.Any(t => t.Uid == child.Uid))
        {
            return;
        }

        _children.Add(child);
        child.Transform.Parent = this;

        UpdateChildPosition(child);
    }

    public GameObject[] GetAllChildren(bool recursion = false)
    {
        var children = new List<GameObject>();

        var childResult = GetAllChildren(this, recursion);
        if (childResult.Length > 0)
        {
            children.AddRange(childResult);
        }

        return children.ToArray();
    }

    private GameObject[] GetAllChildren(Transform current, bool recursion = false)
    {
        var children = new List<GameObject>();

        children.AddRange(current._children);

        if (recursion)
        {
            for (int i = 0; i < current.GetChildCount(); i++)
            {
                var child = GetChild(i);

                var childResult = GetAllChildren(child.Transform, recursion);
                if (childResult.Length > 0)
                {
                    children.AddRange(childResult);
                }
            }
        }

        return children.ToArray();
    }

    public void SetParent(GameObject parent)
    {
        parent.Transform.AddChild(this.Owner);
    }

    public int GetChildCount() => _children.Count;

    /// <summary>
    /// 更新渲染规格
    /// </summary>
    public void UpdateRenderRect()
    {
        Rect = GetRenderRect();

        foreach (var child in _children)
        {
            child.Transform.UpdateRenderRect();
        }
    }

    private RectTransform GetRenderRect()
    {
        var hasParent = Parent != null;

        var pivotPos = GetPivotPos();

        /*
            如果Transform.AnchorMin和Max刚好都是0.5，则以pivot的位置和Size的大小绘制组件；
            否则，就是相对布局：
                AnchorMin.x：代表相对于父组件左边的距离比例起点
                AnchorMin.y: 代表相对于父组件顶部的距离比例起点
                AnchorMax.x: 代表相对于父组件左边的距离比例终点
                AnchorMax.y: 代表相对于父组件顶部的距离比例终点
         */

        // 相对于父组件左边的距离比例起点
        var startX = hasParent == false ? 0 : AnchorMin.X * Parent.Size.Width;

        // 相对于父组件左边的距离比例终点
        var endX = hasParent == false ? Size.Width : AnchorMax.X * Parent.Size.Width;

        // 相对于父组件顶部的距离比例起点
        var startY = hasParent == false ? 0 : AnchorMin.Y * Parent.Size.Height;

        // 相对于父组件顶部的距离比例终点
        var endY = hasParent == false ? Size.Height : AnchorMax.Y * Parent.Size.Height;

        // 坐标再加上left、right、top、bottom偏移
        startX = startX + pivotPos.X + OffsetMin.X;
        endX = endX + pivotPos.X - OffsetMax.X;

        startY = startY + pivotPos.Y + OffsetMin.Y;
        endY = endY + pivotPos.Y - OffsetMax.Y;

        if (startX == endX)
        {
            endX += Size.Width;
        }

        if (startY == endY)
        {
            endY += Size.Height;
        }

        int sizeX = (int)(endX - startX);
        int sizeY = (int)(endY - startY);

        return new RectTransform(startX, startY, sizeX, sizeY);
    }

    private Vector2 GetPivotPos()
    {
        var distanceX = Size.Width * Pivot.X;
        var distanceY = Size.Height * Pivot.Y;

        Vector2 pos;

        if (Parent != null)
        {
            var parentRenderRect = Parent.Rect;
            pos = new Vector2(parentRenderRect.X + LocalPosition.X, parentRenderRect.Y + LocalPosition.Y);
        }
        else
        {
            pos = Position;
        }

        return new Vector2(pos.X - distanceX, pos.Y - distanceY);
    }

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
