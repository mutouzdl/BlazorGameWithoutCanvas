using Blazor.Extensions.Canvas.Canvas2D;

namespace BlazorGameFramework;

public class Animator : BaseComponent
{
    private readonly IDictionary<string, Animation> _animations;

    private Animation? _currentAnimation;

    public Animator(GameObject owner) : base(owner)
    {
        _animations = new Dictionary<string, Animation>();
    }

    public string Name { get; }

    public Animation? GetAnimation(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || _animations.ContainsKey(name) == false)
        {
            return null;
        }

        return _animations[name];
    }

    public void AddAnimation(Animation animation)
    {
        if (animation == null)
            throw new ArgumentNullException(nameof(animation));
        if (_animations.ContainsKey(animation.Name))
            throw new ArgumentException($"there is already an animation with the same name: {animation.Name}");

        _animations.Add(animation.Name, animation);
    }

    public void Play(string name)
    {
        _currentAnimation = GetAnimation(name);
        if (_currentAnimation != null)
        {
            _currentAnimation.Play();
        }
    }

    public async ValueTask Render(GameContext game, Canvas2DContext context)
    {
        if (_currentAnimation == null || _currentAnimation.Playing == false)
        {
            return;
        }

        await _currentAnimation.Render(game, context);
    }
}