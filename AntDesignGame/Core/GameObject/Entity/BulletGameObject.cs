using System.Drawing;
using AntDesignGameFramework;

namespace AntDesignGame;


public class BulletGameObject : GameObject
{
    public float Speed { get; set; }
    public int Atk { get; set; }
    public GameObject Target { get; private set; }
    public string BulletId { get; private set; }


    public BulletGameObject(Type webComponentType, string bulletId, GameObject target) : base(webComponentType)
    {
        BulletId = bulletId;
        Target = target;
    }

    public void Init()
    {
        var spriteGameObject = new SpriteGameObject(typeof(Sprite));

        spriteGameObject.Transform.Size = new Size(16, 16);
        spriteGameObject.AssetName = $"assets/bullet/bullet{BulletId}.png";

        Transform.AddChild(spriteGameObject);

        var boundingBox = new BoundingBox(this);
        boundingBox.SetSize(spriteGameObject.Transform.Size);
        AddComponent(boundingBox);

        var linerMove = new LinerMoveComponent(this, Target.Transform.Position, Transform.Direction, Speed, true);
        AddComponent(linerMove);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (IsDestroy)
        {
            return;
        }

        if (collision.GameObject is ActorGameObject
            && collision.GameObject.Uid != this.Transform.Parent.Owner.Uid
            && RelationUtility.GetRelation(collision.GameObject.Tag, Transform.Parent.Owner.Tag) == EnumActorRelation.敌对
            )
        {
            Destroy();
        }
    }
}
