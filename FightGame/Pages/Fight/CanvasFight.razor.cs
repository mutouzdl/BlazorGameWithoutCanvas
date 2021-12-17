﻿using System.Numerics;
using Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FightGame
{
    public partial class CanvasFight : ComponentBase
    {
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        [Inject]
        public IGraphicAssetService GraphicAssetService { get; set; }

        private BECanvasComponent _canvas;
        private ElementReference _spritesheet;

        private DemoGame _game;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
                return;

            await JsRuntime.InvokeAsync<object>("initCanvas", DotNetObjectReference.Create(this));

            _game = await DemoGame.Create(_canvas);

            Global.GameContext = _game;
            Global.GraphicAssetService = GraphicAssetService;

            var hero = new Actor();
            hero.Init("1064020302");
            hero.Transform.Direction = Vector2.UnitX * -1;
            hero.Transform.Position = new Vector2(
                0,
                _game.Display.Size.Height / 2 - hero.Transform.Size.Height / 2);

            _game.AddGameObject(hero);

            var monster = new Actor();
            monster.Init("1019010301");
            monster.Transform.Position = new Vector2(
                _game.Display.Size.Width - monster.Transform.Size.Width,
                _game.Display.Size.Height / 2 - monster.Transform.Size.Height / 2);

            _game.AddGameObject(monster);

        }

        [JSInvokable]
        public async ValueTask GameLoop(float timeStamp, float elapsedTime)
        {
            if (null == _game) return;
            await _game.Step(timeStamp, elapsedTime);
        }
    }
}
