using System.Drawing;
using System.Numerics;
using AntDesignGameFramework;
using AntDesignGameFramework.Utility;

namespace AntDesignGame;


public class BulletGameObject : GameObject
{
    public float Speed { get; set; }
    public int Atk { get; set; }
    public GameObject Target { get; set; }
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

        var boundingBox = new BoundingBox(this);
        boundingBox.SetSize(spriteGameObject.Transform.Size);
        AddComponent(boundingBox);
    }

    protected override async ValueTask OnUpdate(GameContext game)
    {
        if (IsDestroy)
        {
            return;
        }

        if (game.Display.IsOutOfRange(Transform.Rect))
        {
            Destroy();
            return;
        }

        var offsetPos = GetNextOffsetPos(game);
        Transform.LocalPosition = new Vector2(Transform.LocalPosition.X + offsetPos.X, Transform.LocalPosition.Y + offsetPos.Y);

    }

    private Vector2 GetNextOffsetPos(GameContext game)
    {
        // 斜率
        var k = Utility.Maths.LinerEquation.CountK(Transform.Position, Target.Transform.Position);

        // 距离
        var d = Speed * game.GameTime.ElapsedTime;//* Transform.Direction.X;

        // 当前点
        var p1 = Transform.Position;

        var p2 = Utility.Maths.LinerEquation.GetP2(k, d, p1);

        return new Vector2(p2.X - Transform.Position.X, p2.Y - Transform.Position.Y) * Transform.Direction.X;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (IsDestroy)
        {
            return;
        }

        if (collision.GameObject is ActorGameObject && collision.GameObject.Uid != this.Transform.Parent.Owner.Uid)
        {
            Destroy();
        }
    }
}
