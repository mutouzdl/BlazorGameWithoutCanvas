using System.Drawing;
using System.Numerics;
using AntDesignGameFramework;

namespace AntDesignGame;


public class BulletGameObject : GameObject
{
    public float Speed { get; set; }
    public string BulletId { get; private set; }


    public BulletGameObject(Type webComponentType, string bulletId) : base(webComponentType)
    {
        BulletId = bulletId;

        Init();
    }

    private void Init()
    {
        var spriteGameObject = new SpriteGameObject(typeof(Sprite));

        spriteGameObject.Transform.Size = new Size(16, 16);
        spriteGameObject.AssetName = $"assets/bullet/bullet{BulletId}.png";

        Transform.AddChild(spriteGameObject);
    }

    protected override async ValueTask OnUpdate(GameContext game)
    {
        Transform.LocalPosition = new Vector2(
            Transform.LocalPosition.X + (Speed * game.GameTime.ElapsedTime * Transform.Direction.X),
            Transform.LocalPosition.Y);
    }
}
