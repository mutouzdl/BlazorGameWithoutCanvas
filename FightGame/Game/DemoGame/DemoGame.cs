using System.Drawing;

using Blazor.Extensions;

using BlazorGameFramework;

namespace FightGame;


public class DemoGame : GameContext
{
    public List<GameObject> GameObjects { get; } = new();

    private DemoGame()
    {
        Display.Size = new Size(1200, 600);
    }

    public static async ValueTask<DemoGame> Create(BECanvasComponent canvas)
    {
        var game = new DemoGame
        {
            Canvas = await canvas.CreateCanvas2DAsync(),
        };

        return game;
    }

    public void AddGameObject(GameObject gameObject)
    {
        if (!GameObjects.Any(t => t.Uid == gameObject.Uid))
        {
            GameObjects.Add(gameObject);
        }
    }

    protected override async ValueTask Update()
    {
        foreach (var gameObject in GameObjects)
        {
            await gameObject.Update(this);
        }
    }

    protected override async ValueTask Render()
    {
        await Canvas.ClearRectAsync(0, 0, this.Display.Size.Width, this.Display.Size.Height);

        await Canvas.SetFontAsync("20px serif");

        await Canvas.StrokeTextAsync(
            $"FPS: {Math.Round(1 / GameTime.ElapsedTime, 0)}",
            this.Display.Size.Width - 80, 20);

        await Canvas.StrokeTextAsync(
            $"ElapsedTime: {Math.Round(GameTime.ElapsedTime, 2)}",
            this.Display.Size.Width - 180, 80);


        foreach (var gameObject in GameObjects)
        {
            await gameObject.Render(this);

        }
    }
}
