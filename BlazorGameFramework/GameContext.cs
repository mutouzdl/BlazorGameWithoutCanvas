namespace BlazorGameFramework;

public abstract class GameContext
{
    /// <summary>
    /// 参考来源：https://github.com/mizrael/BlazorCanvas
    /// </summary>
    public GameTime GameTime { get; } = new GameTime();
    public Display Display { get; } = new Display();

    public async ValueTask Step(float timeStamp)
    {
        this.GameTime.TotalTime = timeStamp;

        await Update();
        await Render();
    }

    protected abstract ValueTask Update();
    protected abstract ValueTask Render();

}