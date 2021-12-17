using AntDesignGameFramework;
using Microsoft.AspNetCore.Components;

namespace AntDesignGame;

public class PropertyBarGameObject : GameObject
{
    [Parameter]
    public decimal MaxValue { get; set; } = 100;
    [Parameter]
    public decimal CurrentValue { get; set; } = 0;
    [Parameter]
    public EnumPropertyBarType PropertyBarType { get; set; } = EnumPropertyBarType.HP;

    public PropertyBarGameObject(Type webComponentType) : base(webComponentType)
    {
    }

}
