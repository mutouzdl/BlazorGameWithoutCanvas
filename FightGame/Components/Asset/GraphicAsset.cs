using Microsoft.AspNetCore.Components;

namespace FightGame;

/// <summary>
/// 参考来源：https://github.com/aventius-software/BlazorGE
/// </summary>
public class GraphicAsset
{
    public ElementReference ImageElementReference { get; set; }
    public bool IsLoaded { get; set; } = false;
    public Func<DateTime, Task> OnLoadAsync { get; set; }
    public string Uid { get; set; } = Guid.NewGuid().ToString();
    public string Url { get; set; }
}
