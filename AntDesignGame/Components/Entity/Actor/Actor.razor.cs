using Microsoft.AspNetCore.Components;

namespace AntDesignGame;


public partial class Actor : ComponentBase
{
    [Parameter]
    public ActorGameObject GameObject { get; set; }

    private string Style => $"position: absolute;" +
        $"left:{GameObject.Transform.Position.X}px;top:{GameObject.Transform.Position.Y}px; ";

    private string ActorStyle => $"background: url('assets/actor/{GameObject.ActorId}.png') no-repeat; {MirrorStyle}";

    private string MirrorStyle => GameObject.Mirror ? "transform: rotateY(180deg);" : "";
}