
using Microsoft.AspNetCore.Components;

namespace AntDesignGame;

public partial class PropertyBar
{
    [Parameter]
    public PropertyBarGameObject GameObject { get; set; }

    private decimal Percent => GameObject.CurrentValue / GameObject.MaxValue * 100;
    private string StrokeColor => GameObject.PropertyBarType switch
    {
        EnumPropertyBarType.HP => "#FF4136",
        EnumPropertyBarType.MP => "#0074D9",
        _ => "#FF4136",
    };

    private string Style => $"width: {GameObject.Transform.Size.Width}px;";
}
