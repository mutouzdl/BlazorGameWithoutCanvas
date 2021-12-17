using AntDesignGameFramework;

namespace AntDesignGame;

public partial class GameWorld
{
    public GameContext GameContext { get; set; }

    public void Refresh()
    {
        StateHasChanged();
    }
}
