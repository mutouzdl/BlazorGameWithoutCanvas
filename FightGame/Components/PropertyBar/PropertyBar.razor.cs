
using Microsoft.AspNetCore.Components;

namespace FightGame;

public partial class PropertyBar
{
    [Parameter]
    public decimal MaxValue { get; set; } = 100;
    [Parameter]
    public decimal CurrentValue { get; set; } = 0;
    [Parameter]
    public EnumPropertyBarType PropertyBarType { get; set; } = EnumPropertyBarType.HP;

    private decimal _percent => CurrentValue / MaxValue * 100;
    private string _strokeColor => PropertyBarType switch
    {
        EnumPropertyBarType.HP => "#FF4136",
        EnumPropertyBarType.MP => "#0074D9",
        _ => "#FF4136",
    };

}
