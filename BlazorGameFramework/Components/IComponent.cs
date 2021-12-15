namespace BlazorGameFramework;

/// <summary>
/// 参考来源：https://github.com/mizrael/BlazorCanvas
/// </summary>
public interface IComponent
{
    ValueTask Update(GameContext game);

    public GameObject Owner { get; }
}