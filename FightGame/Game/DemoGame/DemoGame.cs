using System.Drawing;

using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;

using BlazorGameFramework;

namespace FightGame;


public class DemoGame : GameContext
{

    private Canvas2DContext _context;
    public List<GameObject> _gameObjects { get; } = new();

    private DemoGame()
    {
        Display.Size = new Size(1200, 600);
    }

    public static async ValueTask<DemoGame> Create(BECanvasComponent canvas)
    {
        var game = new DemoGame { _context = await canvas.CreateCanvas2DAsync() };

        return game;
    }

    public void AddGameObject(GameObject gameObject)
    {
        if (!_gameObjects.Any(t => t.Uid == gameObject.Uid))
        {
            _gameObjects.Add(gameObject);
        }
    }

    protected override async ValueTask Update()
    {
        foreach (var gameObject in _gameObjects)
        {
            await gameObject.Update(this);
        }
    }

    protected override async ValueTask Render()
    {
        await _context.ClearRectAsync(0, 0, this.Display.Size.Width, this.Display.Size.Height);

        foreach (var gameObject in _gameObjects)
        {
            var animator = gameObject.GetComponent<Animator>();
            if (animator != null)
            {
                await animator.Render(this, _context);
            }
        }
    }
}
