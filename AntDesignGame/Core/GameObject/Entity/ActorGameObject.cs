using AntDesignGameFramework;

namespace AntDesignGame;


public class ActorGameObject : GameObject
{
    public string ActorId { get; set; }

    public bool Mirror { get; set; } = false;

    public EnumActorState State { get; private set; } = EnumActorState.Stand;

    private BulletManager _bulletManager;

    private double _timeCounter = 0;

    public ActorGameObject(Type webComponentType) : base(webComponentType)
    {
    }

    protected override async ValueTask OnUpdate(GameContext game)
    {
        if (_bulletManager != null)
        {
            _bulletManager.Logic(game.GameTime.ElapsedTime);

            _timeCounter += game.GameTime.ElapsedTime;

            if (_timeCounter > 2)
            {
                _timeCounter = 0;

                _bulletManager.Fire();
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
