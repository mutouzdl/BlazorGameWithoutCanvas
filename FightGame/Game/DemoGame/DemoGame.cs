using System.Drawing;
using System.Numerics;

using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;

using BlazorGameFramework;

namespace FightGame;


public class DemoGame : GameContext
{

    private Canvas2DContext _context;
    public GameObject Hero { get; private set; }

    private DemoGame()
    {
        Display.Size = new Size(1200, 800);
    }

    public static async ValueTask<DemoGame> Create(BECanvasComponent canvas)
    {
        var hero = new GameObject();

        hero.AddComponent(new Transform(hero)
        {
            Position = Vector2.Zero,
            Direction = Vector2.One,
            Size = new Size(64, 64)
        });

        var game = new DemoGame { _context = await canvas.CreateCanvas2DAsync(), Hero = hero };

        return game;
    }

    protected override async ValueTask Update()
    {
        await Hero.Update(this);
    }

    protected override async ValueTask Render()
    {
        await _context.ClearRectAsync(0, 0, this.Display.Size.Width, this.Display.Size.Height);

        var animator = Hero.GetComponent<Animator>();
        if (animator != null)
        {
            await animator.Render(this, _context);
        }
    }
}
