using System.Drawing;
using System.Numerics;
using AntDesignGameFramework;
using Microsoft.JSInterop;

namespace AntDesignGame;


public class ActorGameObject : GameObject
{
    public string ActorId { get; set; }

    /// <summary>
    /// 图片是否镜像翻转
    /// </summary>
    public bool ImageMirror { get; set; } = false;

    /// <summary>
    /// TODO 临时的HP变量，以后要封装
    /// </summary>
    public int HP { get; set; } = 100;

    public EnumActorState State { get; private set; } = EnumActorState.Stand;

    private PropertyBarGameObject _propertyBarGameObject;
    private BulletManager _bulletManager;

    private double _timeCounter = 3;

    public ActorGameObject(Type webComponentType) : base(webComponentType)
    {
        Init();
    }

    private void Init()
    {
        Transform.Size = new Size(64, 64);

        _propertyBarGameObject = new(typeof(PropertyBar));

        _propertyBarGameObject.Transform.Pivot = new Vector2(0.5f, 1);
        _propertyBarGameObject.Transform.AnchorMin = new Vector2(0.5f, 0);
        _propertyBarGameObject.Transform.AnchorMax = new Vector2(0.5f, 0);
        _propertyBarGameObject.Transform.LocalPosition = new Vector2(0, -0);
        _propertyBarGameObject.Transform.Size = new Size(100, 30);
        _propertyBarGameObject.MaxValue = HP;
        _propertyBarGameObject.CurrentValue = HP;

        Transform.AddChild(_propertyBarGameObject);

        var boundingBox = new BoundingBox(this);
        boundingBox.SetSize(Transform.Size);
        AddComponent(boundingBox);

        _bulletManager = new BulletManager();
        AddComponent(_bulletManager);
    }

    [JSInvokable]
    public void AnimationStart(JSAnimationEvent e)
    {
    }

    [JSInvokable]
    public void AnimationEnd(JSAnimationEvent e)
    {
        if (State != EnumActorState.Stand)
        {
            Stand();
        }
    }

    protected override async ValueTask OnUpdate(GameContext game)
    {
        if (_bulletManager != null)
        {
            _timeCounter += game.GameTime.ElapsedTime;

            if (_timeCounter > 3)
            {
                _timeCounter = 0;

                var bulletGameObject = _bulletManager.GetOrAddBulletGameObject(Transform.Direction);

                Transform.AddChild(bulletGameObject);

                Attack();
            }
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.GameObject is BulletGameObject && collision.GameObject.Transform.Parent.Owner.Uid != this.Uid)
        {
            HP -= 2;

            if (HP < 0)
            {
                HP = 0;
            }

            _propertyBarGameObject.CurrentValue = HP;
            ReceiveHurt();
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
