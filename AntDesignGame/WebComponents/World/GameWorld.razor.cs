using AntDesignGameFramework;

namespace AntDesignGame;

public partial class GameWorld
{
    public GameContext GameContext { get; private set; }

    private RenderGameWordObject _renderGameWorldObject;

    public void SetGameContext(GameContext gameContext)
    {
        GameContext = gameContext;

        GameContext.OnAddGameObject += (gameObject) =>
        {
        };

        GameContext.OnDestoryGameObject += (gameObject) =>
        {
        };
    }

    public void Refresh()
    {
        StateHasChanged();
    }
}
