using Microsoft.AspNetCore.Components;

namespace FightGame
{
    public partial class Actor
    {
        [Parameter]
        public string ActorId { get; set; }

        [Parameter]
        public bool Mirror { get; set; } = false;

        private EnumActorState _state = EnumActorState.Stand;
        private BulletManager _bulletManager;

        private double _timeCounter = 0;

        public void Logic(double deltaTime)
        {
            if (_bulletManager != null)
            {
                _bulletManager.Logic(deltaTime);

                _timeCounter += deltaTime;

                if (_timeCounter > 1)
                {
                    _timeCounter = 0;

                    _bulletManager.Fire();
                }

                StateHasChanged();
            }
        }

        public async void Stand()
        {
            _state = EnumActorState.Stand;
            StateHasChanged();
        }

        public async void Attack()
        {
            _state = EnumActorState.Attack;
            StateHasChanged();

            await Task.Delay(500);

            Stand();
        }

        public async void ReceiveHurt()
        {
            _state = EnumActorState.Hurt;
            StateHasChanged();

            await Task.Delay(300);

            Stand();
        }

        public async void Dead()
        {
            _state = EnumActorState.Dead;
            StateHasChanged();
        }

        public async void Resurgence()
        {
            Stand();
        }

        private string _style => Mirror ? "transform: rotateY(180deg);" : "";
    }
}
