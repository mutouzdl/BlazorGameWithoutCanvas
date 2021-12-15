using System.Numerics;
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

        private int _fps = 0;

        private BECanvasComponent _canvas;
        private ElementReference _spritesheet;

        private DemoGame _game;

        private World _world;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender || _world != null)
                return;

            _game = await DemoGame.Create(_canvas);

            var hero = new Actor(GraphicAssetService);
            hero.Init("1064020302");
            hero.Transform.Direction = Vector2.UnitX * -1;
            hero.Transform.Position = new Vector2(
                0,
                _game.Display.Size.Height / 2 - hero.Transform.Size.Height / 2);

            _game.AddGameObject(hero);


            var monster = new Actor(GraphicAssetService);
            monster.Init("1019010301");
            monster.Transform.Position = new Vector2(
                _game.Display.Size.Width - monster.Transform.Size.Width,
                _game.Display.Size.Height / 2 - monster.Transform.Size.Height / 2);

            _game.AddGameObject(monster);

            _world = new();
            _world.Logic += Logic;
            _world.Start();
        }

        private async Task Logic(object sender, WorldLogicEventArgs e)
        {
            _fps = _world.CurrentFPS;

            StateHasChanged();

            await _game.Step(e.DeltaTime);
        }
    }
}
