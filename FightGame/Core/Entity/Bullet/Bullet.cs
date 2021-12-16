using System.Drawing;
using System.Numerics;
using BlazorGameFramework;

namespace FightGame;


public class Bullet : GameObject
{
    public float Speed { get; set; } = 10;

    public Bullet()
    {
        var asset = Global.GraphicAssetService.LoadGraphicAsset($"assets/bullet/bullet1.png");

        var sprite = new Sprite()
        {
            Size = new Size(16, 16),
            SpriteSheet = asset.ImageElementReference,
        };

        AddComponent(sprite);
    }

    protected override async ValueTask OnUpdate(GameContext game)
    {
        Transform.LocalPosition = new Vector2(
            Transform.LocalPosition.X + Speed * game.GameTime.ElapsedTime,
            Transform.LocalPosition.Y
        );
    }

}
