using Blazor.Extensions.Canvas.Canvas2D;

namespace BlazorGameFramework;

public abstract class GameContext
{
    /// <summary>
    /// 参考来源：https://github.com/mizrael/BlazorCanvas
    /// </summary>
    public GameTime GameTime { get; } = new GameTime();
    public Display Display { get; } = new Display();
    public Canvas2DContext Canvas { get; protected set; }

    //public async ValueTask Step(float timeStamp)
    //{
    //    this.GameTime.TotalTime = timeStamp;

    //    await Update();
    //    await Render();
    //}

    public async ValueTask Step(float timeStamp, float elapsedTime)
    {
        this.GameTime.TotalTime = timeStamp;
        this.GameTime.ElapsedTime = elapsedTime;

        await Update();
        await Render();
    }

    protected abstract ValueTask Update();
    protected abstract ValueTask Render();

}