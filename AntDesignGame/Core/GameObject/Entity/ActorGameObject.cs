using System.Drawing;
using System.Numerics;
using AntDesignGameFramework;

namespace AntDesignGame;


public class ActorGameObject : GameObject
{
    public string ActorId { get; set; }

    /// <summary>
    /// 图片是否镜像翻转
    /// </summary>
    public bool ImageMirror { get; set; } = false;
    public bool IsDead { get; private set; } = false;

    public FightProperty FightProperty { get; private set; } = new FightProperty();

    public EnumActorState State { get; private set; } = EnumActorState.NotInit;

    private PropertyBarGameObject _propertyBarGameObject;
    private BulletManager _bulletManager;

    private double _timeCounter = 3;

    public ActorGameObject(Type webComponentType) : base(webComponentType)
    {
    }

    public void Init()
    {
        Transform.Size = new Size(64, 64);

        _propertyBarGameObject = new(typeof(PropertyBar));

        _propertyBarGameObject.Transform.Pivot = new Vector2(0.5f, 1);
        _propertyBarGameObject.Transform.AnchorMin = new Vector2(0.5f, 0);
        _propertyBarGameObject.Transform.AnchorMax = new Vector2(0.5f, 0);
        _propertyBarGameObject.Transform.LocalPosition = new Vector2(0, -0);
        _propertyBarGameObject.Transform.Size = new Size(100, 30);
        _propertyBarGameObject.MaxValue = FightProperty.HP;
        _propertyBarGameObject.CurrentValue = FightProperty.HP;

        Transform.AddChild(_propertyBarGameObject);

        var boundingBox = new BoundingBox(this);
        boundingBox.SetSize(Transform.Size);
        AddComponent(boundingBox);

        _bulletManager = new BulletManager();
        AddComponent(_bulletManager);

        if (Tag == "hero")
            _timeCounter = FightProperty.AtkDelay;

        State = EnumActorState.Stand;
    }

    protected override void OnAnimationEnd(JSAnimationEvent e)
    {
        if (IsDestroy)
        {
            return;
        }

        if (State == EnumActorState.Dead)
        {
            Destroy();
        }

        if (IsDead)
        {
            return;
        }

        if (State != EnumActorState.Stand)
        {
            Stand();
        }
    }

    protected override async ValueTask OnUpdate(GameContext game)
    {
        if (State == EnumActorState.NotInit)
        {
            return;
        }

        if (IsDestroy || IsDead)
        {
            return;
        }

        if (_bulletManager != null)
        {
            _timeCounter += game.GameTime.ElapsedTime;

            if (_timeCounter > FightProperty.AtkDelay)
            {
                var targets = GetNextAtkTargets(game);

                if (targets.Length == 0)
                {
                    return;
                }

                _timeCounter = 0;

                foreach (var target in targets)
                {
                    var bulletGameObject = _bulletManager.GetOrAddBulletGameObject(
                        new Random().Next(1, 6), Transform.Direction, FightProperty.Atk, target);

                    Transform.AddChild(bulletGameObject);
                }

                Attack();
            }
        }
    }

    private GameObject[] GetNextAtkTargets(GameContext game)
    {
        var objs = new List<GameObject>();
        foreach (var gameObject in game.GameObjects)
        {
            var actorGameObj = gameObject as ActorGameObject;

            if (actorGameObj == null || actorGameObj.IsDead)
            {
                continue;
            }

            var tag = actorGameObj.Tag;
            if ((Tag == Const.Tags.Hero && tag == Const.Tags.Monster)
                || Tag == Const.Tags.Monster && tag == Const.Tags.Hero)
            {
                objs.Add(actorGameObj);
            }
        }

        return objs.ToArray();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (IsDead)
        {
            return;
        }

        if (collision.GameObject is BulletGameObject && collision.GameObject.Transform.Parent.Owner.Uid != this.Uid)
        {
            var bulletGameObject = (BulletGameObject)collision.GameObject;

            FightProperty.HP -= bulletGameObject.Atk - FightProperty.Def;

            if (FightProperty.HP <= 0)
            {
                FightProperty.HP = 0;
                IsDead = true;
            }

            _propertyBarGameObject.CurrentValue = FightProperty.HP;

            if (IsDead)
            {
                Dead();
            }
            else
            {
                ReceiveHurt();
            }
        }
    }

    public async void Stand()
    {
        State = EnumActorState.Stand;
    }

    public async void Attack()
    {
        State = EnumActorState.Attack;
    }

    public async void ReceiveHurt()
    {
        State = EnumActorState.Hurt;
    }

    public async void Dead()
    {
        State = EnumActorState.Dead;
    }

    public async void Resurgence()
    {
        Stand();
    }

}
