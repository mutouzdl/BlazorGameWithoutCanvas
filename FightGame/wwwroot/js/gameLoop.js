function gameLoop(timeStamp) {
  window.requestAnimationFrame(gameLoop);
  game.instance.invokeMethodAsync('GameLoop', timeStamp, game.canvas.width, game.canvas.height);
}

function onResize() {
  if (!window.game.canvas)
    return;

  game.canvas.width = window.innerWidth;
  game.canvas.height = window.innerHeight;
}

window.initGame = (instance) => {
  var canvasContainer = document.getElementById('canvasContainer'),
    canvases = canvasContainer.getElementsByTagName('canvas') || [];
  window.game = {
    instance: instance,
    canvas: canvases.length ? canvases[0] : null
  };

  window.addEventListener("resize", onResize);
  onResize();

  window.requestAnimationFrame(gameLoop);
};