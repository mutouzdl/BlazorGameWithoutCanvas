const FPS = 60;
const PER_FRAME_MILLISECONDS = 1000.0 / FPS;
let lastTime = 0;
let elapsedTime = 0;

function gameLoop(timeStamp) {
  elapsedTime = timeStamp - lastTime;

  lastTime = timeStamp;

  const startTimeStamp = new Date().getTime();
  game.instance.invokeMethodAsync('GameLoop', timeStamp / 1000.0, elapsedTime / 1000.0);
  const endTimeStamp = new Date().getTime();

  const gameLoopCostTime = endTimeStamp - startTimeStamp;
  var delay = PER_FRAME_MILLISECONDS - gameLoopCostTime;
  if (delay < 0) delay = 0;

  setTimeout(() => { window.requestAnimationFrame(gameLoop); }, delay);
}

window.initCanvas = (instance) => {
  var canvasContainer = document.getElementById('canvasContainer'),
    canvases = canvasContainer.getElementsByTagName('canvas') || [];
  window.game = {
    instance: instance,
    canvas: canvases.length ? canvases[0] : null
  };

  if (window.game.canvas) {
    window.game.canvas.onblur = (e) => {
      window.game.canvas.focus();
    };
    window.game.canvas.tabIndex = 0;
    window.game.canvas.focus();

    window.game.canvas.width = 1200;
    window.game.canvas.height = 600;

  }

  window.requestAnimationFrame(gameLoop);

  return {
    Width: window.game.canvas.width,
    Height: window.game.canvas.height
  };
};