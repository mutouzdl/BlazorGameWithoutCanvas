using Microsoft.AspNetCore.Components;

namespace AntDesignGame;

public partial class LetFight : ComponentBase
{
    private World _world = new();

    private int _fps = 0;
    private Actor _hero;
    private Actor _monster;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _world.Logic += Logic;
        _world.Start();
    }

    private async Task Logic(object sender, WorldLogicEventArgs e)
    {
        _fps = _world.CurrentFPS;

        if (_hero != null)
        {
            _hero.Logic(e.DeltaTime);
        }

        if (_monster != null)
        {
            _monster.Logic(e.DeltaTime);
        }

        StateHasChanged();
    }
}
