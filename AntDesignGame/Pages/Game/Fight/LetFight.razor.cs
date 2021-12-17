using System.Drawing;
using System.Numerics;
using Microsoft.AspNetCore.Components;

namespace AntDesignGame;

public partial class LetFight : ComponentBase
{
    private GameLoop _gameLoop;
    private GameWorld _gameWorld;

    private int _fps = 0;
    //private Actor _hero;
    //private Actor _monster;

    protected override void OnInitialized()
    {
        base.OnInitialized();

    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (_gameLoop != null)
        {
            return;
        }

        var gameContext = new DemoGameContext();

        gameContext.Display.Size = new Size(1200, 600);

        ActorGameObject actorGameObject = new ActorGameObject(typeof(Actor))
        {
            ActorId = "1064020302",
            Mirror = true,
        };

        actorGameObject.Transform.Position = new Vector2(10, 120);

        gameContext.AddGameObject(actorGameObject);

        _gameWorld.GameContext = gameContext;
        _gameWorld.Refresh();

        _gameLoop = new();
        _gameLoop.Logic += Logic;
        _gameLoop.Start();
    }

    private async Task Logic(object sender, GameLoopLogicEventArgs e)
    {
        _fps = _gameLoop.CurrentFPS;

        //if (_hero != null)
        //{
        //    _hero.Logic(e.DeltaTime);
        //}

        //if (_monster != null)
        //{
        //    _monster.Logic(e.DeltaTime);
        //}

        await _gameWorld.GameContext.Step(e.ElapsedTime);

        StateHasChanged();
    }
}
