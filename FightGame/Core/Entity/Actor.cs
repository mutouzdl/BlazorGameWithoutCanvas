using System.Drawing;
using System.Numerics;
using BlazorGameFramework;

namespace FightGame
{
    public class Actor : GameObject
    {
        private static readonly Size FrameSize = new Size(64, 64);
        private static readonly Size ImageSize = new Size(576, 384);

        private IGraphicAssetService _graphicAssetService;

        public Actor(IGraphicAssetService graphicAssetService)
        {
            _graphicAssetService = graphicAssetService;
        }

        public void Init(string assetName)
        {
            AddComponent(new Transform(this)
            {
                Position = Vector2.Zero,
                Direction = Vector2.One,
                Size = FrameSize
            });

            var asset = _graphicAssetService.LoadGraphicAsset($"assets/actor/{assetName}.png");

            int startFrame = 0;
            int frameCount = 3;
            int fps = 3;
            string animationName = "stand";

            var animation = new Animation(
                this,
                animationName,
                FrameSize,
                ImageSize,
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
    }
}
