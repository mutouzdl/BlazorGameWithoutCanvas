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
    private LinerMoveComponent _linerMoveComponent;

    private double _timeCounter = 3;

    public ActorGameObject(Type webComponentType) : base(webComponentType)
    {
    }

    public void Init()
    {
        Transform.Size = new Size(64, 64);

        // 血量组件
        _propertyBarGameObject = new(typeof(PropertyBar));

        _propertyBarGameObject.Transform.Pivot = new Vector2(0.5f, 1);
        _propertyBarGameObject.Transform.AnchorMin = new Vector2(0.5f, 0);
        _propertyBarGameObject.Transform.AnchorMax = new Vector2(0.5f, 0);
        _propertyBarGameObject.Transform.LocalPosition = new Vector2(0, -0);
        _propertyBarGameObject.Transform.Size = new Size(100, 30);
        _propertyBarGameObject.MaxValue = FightProperty.HP;
        _propertyBarGameObject.CurrentValue = FightProperty.HP;

        Transform.AddChild(_propertyBarGameObject);

        // 碰撞组件
        var boundingBox = new BoundingBox(this);
        boundingBox.SetSize(Transform.Size);
        AddComponent(boundingBox);

        // 子弹管理器组件
        _bulletManager = new BulletManager();
        AddComponent(_bulletManager);

        // 移动组件
        _linerMoveComponent = new LinerMoveComponent(this, null, Transform.Direction, FightProperty.MoveSpeed, true);
        _linerMoveComponent.Pause();
        AddComponent(_linerMoveComponent);

        // 初次攻击不需要冷却
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
                    _linerMoveComponent.Resume();
                    return;
                }

                // 攻击时暂停移动
                _linerMoveComponent.Pause();

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

            if (RelationUtility.GetRelation(actorGameObj.Tag, Tag) != EnumActorRelation.敌对)
            {
                continue;
            }

            // 距离以水平方向计算
            var distance = Math.Abs(actorGameObj.Transform.Position.X - Transform.Position.X);

            if (distance >= FightProperty.AtkRange)
            {
                continue;
            }

            objs.Add(actorGameObj);
        }

        return objs.ToArray();
    }

    public void ReveiveHurt(int value)
    {
        if (IsDead)
        {
            return;
        }

        FightProperty.HP -= value - FightProperty.Def;

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
            Hurt();
        }
    }

    private async void Stand()
    {
        State = EnumActorState.Stand;
    }

    private async void Attack()
    {
        State = EnumActorState.Attack;
    }

    private async void Hurt()
    {
        State = EnumActorState.Hurt;
    }

    private async void Dead()
    {
        State = EnumActorState.Dead;
    }

    private async void Resurgence()
    {
        Stand();
    }

}
