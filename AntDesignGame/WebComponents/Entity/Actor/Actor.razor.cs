using Microsoft.JSInterop;

namespace AntDesignGame;


public partial class Actor : GameWebComponent<ActorGameObject>
{
    private string ActorStyle => $"background: url('assets/actor/{GameObject.ActorId}.png') no-repeat; {ImageMirrorStyle}";

    private string ImageMirrorStyle => GameObject.ImageMirror ? "transform: rotateY(180deg);" : "";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var dotNetObjRef = DotNetObjectReference.Create(GameObject);
            var jsFuncName = "addDomEventListener";
            var elementSelector = $"#actor_{GameObject.Uid}";

            await Global.JSRuntime.InvokeAsync<object>(
                  jsFuncName,
                  elementSelector,
                  "webkitAnimationStart",
                  dotNetObjRef,
                  "AnimationStart");

            await Global.JSRuntime.InvokeAsync<object>(
                  jsFuncName,
                  elementSelector,
                  "webkitAnimationEnd",
                  dotNetObjRef,
                  "AnimationEnd");
        }
    }

}