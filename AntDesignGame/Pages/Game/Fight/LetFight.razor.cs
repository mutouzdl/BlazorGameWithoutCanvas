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
            ImageMirror = true,
        };
        actorGameObject.Transform.LocalPosition = new Vector2(100, 220);

        ActorGameObject monsterGameObject = new ActorGameObject(typeof(Actor))
        {
            ActorId = "1019010301",
        };
        monsterGameObject.Transform.LocalPosition = new Vector2(1000, 220);
        monsterGameObject.Transform.Direction = Vector2.UnitX * -1;

        gameContext.AddGameObject(actorGameObject);
        //gameContext.AddGameObject(monsterGameObject);

        _gameWorld.SetGameContext(gameContext);
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

        _gameWorld.Refresh();
    }
}
