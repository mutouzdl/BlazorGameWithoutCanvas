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

    public EnumActorState State { get; private set; } = EnumActorState.Stand;

    private PropertyBarGameObject _propertyBarGameObject;
    private BulletManager _bulletManager;

    private double _timeCounter = 0;

    public ActorGameObject(Type webComponentType) : base(webComponentType)
    {
        Init();
    }

    private void Init()
    {
        _propertyBarGameObject = new(typeof(PropertyBar));

        _propertyBarGameObject.Transform.LocalPosition = new Vector2(0, -0);
        _propertyBarGameObject.Transform.Size = new Size(100, 30);
        _propertyBarGameObject.CurrentValue = 80;

        Transform.AddChild(_propertyBarGameObject);

        _bulletManager = new BulletManager();
        AddComponent(_bulletManager);
    }

    protected override async ValueTask OnUpdate(GameContext game)
    {
        if (_bulletManager != null)
        {
            _timeCounter += game.GameTime.ElapsedTime;

            if (_timeCounter > 2)
            {
                _timeCounter = 0;

                var bulletGameObject = _bulletManager.GetOrAddBulletGameObject(Transform.Direction);

                Transform.AddChild(bulletGameObject);
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
