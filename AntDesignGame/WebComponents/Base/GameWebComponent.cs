using AntDesignGameFramework;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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

    protected async Task ListenAnimationStart()
    {

        var dotNetObjRef = DotNetObjectReference.Create(GameObject);
        var jsFuncName = "addDomEventListener";
        var elementSelector = $"#anim_{GameObject.Uid}";

        await Global.JSRuntime.InvokeAsync<object>(
              jsFuncName,
              elementSelector,
              "webkitAnimationStart",
              dotNetObjRef,
              "AnimationStart");
    }

    protected async Task ListenAnimationIteration()
    {
        var dotNetObjRef = DotNetObjectReference.Create(GameObject);
        var jsFuncName = "addDomEventListener";
        var elementSelector = $"#anim_{GameObject.Uid}";

        await Global.JSRuntime.InvokeAsync<object>(
              jsFuncName,
              elementSelector,
              "webkitAnimationIteration",
              dotNetObjRef,
              "AnimationIteration");
    }

    protected async Task ListenAnimationEnd()
    {

        var dotNetObjRef = DotNetObjectReference.Create(GameObject);
        var jsFuncName = "addDomEventListener";
        var elementSelector = $"#anim_{GameObject.Uid}";

        await Global.JSRuntime.InvokeAsync<object>(
              jsFuncName,
              elementSelector,
              "webkitAnimationEnd",
              dotNetObjRef,
              "AnimationEnd");
    }
}
