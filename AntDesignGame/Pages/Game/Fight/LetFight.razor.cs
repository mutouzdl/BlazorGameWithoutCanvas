using System.Drawing;
using System.Numerics;
using AntDesign.JsInterop;
using AntDesignGameFramework;
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

        // 创建英雄
        ActorGameObject heroGameObject = new ActorGameObject(typeof(Actor))
        {
            ActorId = "1064020302",
            ImageMirror = true,
            Tag = Const.Tags.Hero,
        };
        heroGameObject.Transform.LocalPosition = new Vector2(100, 220);
        heroGameObject.FightProperty.Atk = 15;
        heroGameObject.FightProperty.AtkRange = 800;
        heroGameObject.FightProperty.AtkNum = 2;
        heroGameObject.FightProperty.AtkDelay = 3.5f;
        heroGameObject.FightProperty.Def = 1;
        heroGameObject.FightProperty.HP = 100;
        heroGameObject.FightProperty.MoveSpeed = 10;
        heroGameObject.Init();

        gameContext.AddGameObject(heroGameObject);

        // 创建敌人
        gameContext.AddGameObject(CreateMonster(Const.Tags.Monster, 400, 320, 350));
        gameContext.AddGameObject(CreateMonster(Const.Tags.Monster, 500, 320, 250));
        gameContext.AddGameObject(CreateMonster(Const.Tags.Monster, 800, 520, 150));

        _gameWorld.SetGameContext(gameContext);
        _gameWorld.Refresh();

        _gameLoop = new();
        _gameLoop.Logic += Logic;
        _gameLoop.Start();
    }

    private ActorGameObject CreateMonster(string tag, float x, float y, int hp)
    {

        ActorGameObject monsterGameObject = new ActorGameObject(typeof(Actor))
        {
            ActorId = "1019010301",
            Tag = tag,
        };
        monsterGameObject.Transform.LocalPosition = new Vector2(x, y);
        monsterGameObject.Transform.Direction = Vector2.UnitX * -1;
        monsterGameObject.FightProperty.Atk = 3;
        monsterGameObject.FightProperty.AtkNum = 1;
        monsterGameObject.FightProperty.AtkRange = 250;
        monsterGameObject.FightProperty.AtkDelay = 2;
        monsterGameObject.FightProperty.Def = 1;
        monsterGameObject.FightProperty.HP = hp;
        monsterGameObject.FightProperty.MoveSpeed = 20;
        monsterGameObject.Init();

        Console.WriteLine($"创建怪物，UID:{monsterGameObject.Uid} Tag:{monsterGameObject.Tag}/{tag}");
        return monsterGameObject;
    }

    private async Task Logic(object sender, GameLoopLogicEventArgs e)
    {
        _fps = 1.0f / e.ElapsedTime;

        await _gameWorld.GameContext.Step(e.ElapsedTime);

        _gameWorld.Refresh();

        StateHasChanged();
    }
}
