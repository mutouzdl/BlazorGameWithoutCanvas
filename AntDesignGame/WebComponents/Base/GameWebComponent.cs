using AntDesignGameFramework;
using Microsoft.AspNetCore.Components;

namespace AntDesignGame;


public class GameWebComponent<TGameObject> : ComponentBase
    where TGameObject : GameObject
{
    [Parameter]
    public RenderFragment Children { get; set; }


    [Parameter]
    public TGameObject GameObject { get; set; }

    protected string GetPositionStyle()
    {
        var rect = GameObject.Transform.Rect;

        return $"position: absolute;" +
            $"left:{rect.X}px;top:{rect.Y}px; " +
            $"width:{rect.Width}px;height:{rect.Height}px; "
            ;
    }

}
