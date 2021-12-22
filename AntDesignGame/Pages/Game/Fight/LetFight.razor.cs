using System.Drawing;
using System.Numerics;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesignGame;

public partial class LetFight : ComponentBase
{
    private GameLoop _gameLoop;
    private GameWorld _gameWorld;

    private float _fps = 0;

    [Inject]
    private IDomEventListener DomEventListener { get; set; }
    [Inject]
    private IJSRuntime JSRuntime { get; set; }

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

        Global.GameContext = gameContext;
        Global.DomEventListener = DomEventListener;
        Global.JSRuntime = JSRuntime;

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
        monsterGameObject.Transform.LocalPosition = new Vector2(800, 220);
        monsterGameObject.Transform.Direction = Vector2.UnitX * -1;

        gameContext.AddGameObject(actorGameObject);
        gameContext.AddGameObject(monsterGameObject);

        _gameWorld.SetGameContext(gameContext);
        _gameWorld.Refresh();

        _gameLoop = new();
        _gameLoop.Logic += Logic;
        _gameLoop.Start();
    }

    private async Task Logic(object sender, GameLoopLogicEventArgs e)
    {
        _fps = 1.0f / e.ElapsedTime;

        await _gameWorld.GameContext.Step(e.ElapsedTime);

        _gameWorld.Refresh();

        StateHasChanged();
    }
}
