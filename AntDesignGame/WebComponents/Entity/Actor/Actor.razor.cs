namespace AntDesignGame;


public partial class Actor : GameWebComponent<ActorGameObject>
{
    private string ActorStyle => $"background: url('assets/actor/{GameObject.ActorId}.png') no-repeat; {ImageMirrorStyle}";

    private string ImageMirrorStyle => GameObject.ImageMirror ? "transform: rotateY(180deg);" : "";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await ListenAnimationEnd();
        }
    }

}