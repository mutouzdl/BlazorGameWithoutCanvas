using System.Drawing;
using BlazorGameFramework;

namespace FightGame
{
    public class Actor : GameObject
    {
        private static readonly Size _frameSize = new Size(64, 64);
        private static readonly Size _imageSize = new Size(576, 384);

        private EnumActorState _state = EnumActorState.Stand;
        private BulletManager _bulletManager;

        private double _timeCounter = 0;

        public Actor()
        {
            _bulletManager = new(this);
        }

        public void Init(string assetName)
        {
            Transform.Size = _frameSize;

            var asset = Global.GraphicAssetService.LoadGraphicAsset($"assets/actor/{assetName}.png");

            int startFrame = 0;
            int frameCount = 3;
            int fps = 3;
            string animationName = "stand";

            var animation = new Animation(
                this,
                animationName,
                _frameSize,
                _imageSize,
                startFrame,
                frameCount,
                fps,
                asset.ImageElementReference
                );

            var animator = new Animator(this);
            animator.AddAnimation(animation);

            AddComponent(animator);

            animator.Play("stand");
        }


        //public async void Stand()
        //{
        //    _state = EnumActorState.Stand;
        //}

        //public async void Attack()
        //{
        //    _state = EnumActorState.Attack;

        //    Stand();
        //}

        //public async void ReceiveHurt()
        //{
        //    _state = EnumActorState.Hurt;

        //    await Task.Delay(300);

        //    Stand();
        //}

        //public async void Dead()
        //{
        //    _state = EnumActorState.Dead;
        //}

        //public async void Resurgence()
        //{
        //    Stand();
        //}


        protected override async ValueTask OnUpdate(GameContext game)
        {
            await _bulletManager.Update(game);

            _timeCounter += game.GameTime.ElapsedTime;

            if (_timeCounter > 1)
            {
                _timeCounter = 0;

                var bullet = _bulletManager.GetOrAddBullet(Transform.Direction * -1);

                bullet.Transform.SetParent(this);
            }
        }
    }
}
