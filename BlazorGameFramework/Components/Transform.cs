using System.Drawing;
using System.Numerics;

namespace BlazorGameFramework;

/// <summary>
/// 参考来源：https://github.com/mizrael/BlazorCanvas
/// </summary>
public class Transform : BaseComponent
{
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Direction { get; set; } = Vector2.UnitX;
    public Size Size { get; set; } = Size.Empty;

    private Rectangle _boundingBox;
    public Rectangle BoundingBox => _boundingBox;

    public Transform(GameObject owner) : base(owner)
    {
    }

    public override async ValueTask Update(GameContext game)
    {
        _boundingBox.Size = this.Size;
        _boundingBox.X = (int)this.Position.X;
        _boundingBox.Y = (int)this.Position.Y;
    }
}
