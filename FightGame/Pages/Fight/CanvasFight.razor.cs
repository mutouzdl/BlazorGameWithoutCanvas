// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Drawing;
using Blazor.Extensions;
using BlazorGameFramework;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FightGame
{
    public partial class CanvasFight : ComponentBase
    {
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        private int _fps = 0;

        private BECanvasComponent _canvas;
        private ElementReference _spritesheet;

        private DemoGame _game;

        private World _world;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender || _world != null)
                return;

            //await JsRuntime.InvokeAsync<object>("initGame", DotNetObjectReference.Create(this));

            _game = await DemoGame.Create(_canvas);

            var animation = new Animation(
                _game.Hero,
                "stand",
                new Size(64, 64),
                new Size(576, 384),
                0,
                3,
                3,
                _spritesheet
                );

            var animator = new Animator(_game.Hero);
            animator.AddAnimation(animation);

            _game.Hero.AddComponent(animator);

            animator.Play("stand");

            _world = new();
            _world.Logic += Logic;
            _world.Start();

        }

        private async void Logic(object sender, WorldLogicEventArgs e)
        {
            _fps = _world.CurrentFPS;

            StateHasChanged();

            await _game.Step(e.DeltaTime);
        }
    }
}
