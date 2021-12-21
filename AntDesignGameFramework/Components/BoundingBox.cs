using System.Drawing;

namespace AntDesignGameFramework;

/// <summary>
/// 参考来源：https://github.com/mizrael/BlazorCanvas
/// </summary>
public class BoundingBox : BaseComponent
{
    private readonly Transform _transform;
    private Rectangle _bounds;
    private Size _halfSize;

    public BoundingBox(GameObject owner) : base(owner)
    {
        _transform = owner.GetComponent<Transform>();
    }

    public Rectangle Bounds => _bounds;

    private Dictionary<string, Collision> _collisions = new();

    public void SetSize(Size size)
    {
        _bounds.Size = size;
        _halfSize = size / 2;
    }

    public override async ValueTask Update(GameContext game)
    {
        var x = (int)_transform.Position.X - _halfSize.Width;
        var y = (int)_transform.Position.Y - _halfSize.Height;

        var changed = _bounds.X != x || _bounds.Y != y;
        _bounds.X = x;
        _bounds.Y = y;
    }

    public void BindCollisionBox(BoundingBox otherBoundingBox)
    {
        if (_collisions.TryGetValue(otherBoundingBox.Owner.Uid, out Collision existsCollison))
        {
            // 同一个碰撞对象
            if (existsCollison.GameObject.Uid == otherBoundingBox.Owner.Uid)
            {
                Owner.OnCollisionStay(existsCollison);
            }
        }
        else
        {
            var collision = new Collision()
            {
                GameObject = otherBoundingBox.Owner,
                BoundingBox = otherBoundingBox,
            };

            Owner.OnCollisionEnter(collision);

            _collisions[otherBoundingBox.Owner.Uid] = collision;
        }
    }

    public void ExitCollisionBox(BoundingBox otherBoundingBox)
    {
        if (_collisions.TryGetValue(otherBoundingBox.Owner.Uid, out Collision existsCollison) == false)
        {
            return;
        }

        // 上一次产生了碰撞，本次没有碰撞，则调用OnCollisionExit回调
        if (existsCollison.GameObject.Uid == otherBoundingBox.Owner.Uid)
        {
            Owner.OnCollisionExit(existsCollison);

            _collisions.Remove(otherBoundingBox.Owner.Uid);
        }
    }
}
