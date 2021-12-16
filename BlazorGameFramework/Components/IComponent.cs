namespace BlazorGameFramework;

/// <summary>
/// 参考来源：https://github.com/mizrael/BlazorCanvas
/// </summary>
public interface IComponent
{
    ValueTask Update(GameContext game);
    ValueTask Render(GameContext game);

    public void SetOwner(GameObject owner);

}