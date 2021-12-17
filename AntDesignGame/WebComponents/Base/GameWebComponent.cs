using AntDesignGameFramework;
using Microsoft.AspNetCore.Components;

namespace AntDesignGame;


public class GameWebComponent<T_GameObject> : ComponentBase
    where T_GameObject : GameObject
{
    [Parameter]
    public RenderFragment Children { get; set; }


    [Parameter]
    public T_GameObject GameObject { get; set; }

    protected string PositionStyle => $"position: absolute;" +
        $"left:{GameObject.Transform.Position.X}px;top:{GameObject.Transform.Position.Y}px; ";

}
