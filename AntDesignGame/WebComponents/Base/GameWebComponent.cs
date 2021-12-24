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

    ~GameWebComponent()
    {
        OnDispose();
    }

    protected virtual void OnDispose()
    {

    }

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
        await AddAnimationEventListener("Start");
    }

    protected async Task ListenAnimationIteration()
    {
        await AddAnimationEventListener("Iteration");
    }

    protected async Task ListenAnimationEnd()
    {
        await AddAnimationEventListener("End");
    }

    protected async Task RemoveListenAnimationStart()
    {
        await RemoveAnimationEventListener("Start");
    }

    protected async Task RemoveListenAnimationIteration()
    {
        await RemoveAnimationEventListener("Iteration");
    }

    protected async Task RemoveListenAnimationEnd()
    {
        await RemoveAnimationEventListener("End");
    }

    private async Task AddAnimationEventListener(string state)
    {
        var jsFuncName = "addDomEventListener";
        var elementSelector = $"#anim_{GameObject.Uid}";

        await Global.JSRuntime.InvokeAsync<object>(
              jsFuncName,
              elementSelector,
              $"webkitAnimation{state}",
              DotNetObjectRef,
              $"Animation{state}");
    }

    private async Task RemoveAnimationEventListener(string state)
    {
        var jsFuncName = "removeDomEventListener";
        var elementSelector = $"#anim_{GameObject.Uid}";

        await Global.JSRuntime.InvokeAsync<object>(
              jsFuncName,
              elementSelector,
              $"webkitAnimation{state}");
    }

    private DotNetObjectReference<TGameObject> _dotNetObjectRef;
    private DotNetObjectReference<TGameObject> DotNetObjectRef
    {
        get
        {
            _dotNetObjectRef = _dotNetObjectRef ?? DotNetObjectReference.Create(GameObject);

            return _dotNetObjectRef;
        }
    }
}
