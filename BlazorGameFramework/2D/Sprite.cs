using System.Drawing;

using Microsoft.AspNetCore.Components;

namespace BlazorGameFramework;

public class Sprite : BaseComponent
{
    public Size Size { get; set; }
    public ElementReference SpriteSheet { get; set; }

    public override async ValueTask Render(GameContext game)
    {
        await game.Canvas.DrawImageAsync(SpriteSheet, Owner.Transform.Position.X, Owner.Transform.Position.Y, Size.Width, Size.Height);
    }
}
