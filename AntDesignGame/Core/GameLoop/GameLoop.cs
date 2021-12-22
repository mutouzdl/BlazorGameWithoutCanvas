using System.Diagnostics;

namespace AntDesignGame;

public class GameLoop
{
    public const int FPS = 30;
    public int FPS_DELAY => Convert.ToInt32(1000.0 / FPS);

    private bool _isRunning = false;
    private decimal _timeCounter = 0;

    public event Func<object, GameLoopLogicEventArgs, Task> Logic;

    public async void Start()
    {
        if (_isRunning)
        {
            return;
        }

        _isRunning = true;

        Stopwatch deltaWatch = new Stopwatch();
        Stopwatch stopWatch = new Stopwatch();
        float deltaTime = 0;
        while (_isRunning || _timeCounter > 0)
        {
            _timeCounter = _isRunning ? 10 : _timeCounter - 1;

            stopWatch.Reset();
            stopWatch.Start();

            deltaWatch.Stop();

            deltaTime = (float)deltaWatch.Elapsed.TotalSeconds;

            deltaWatch.Reset();
            deltaWatch.Start();
            await OnLogic(deltaTime);

            stopWatch.Stop();

            var logicCostMilliseconds = stopWatch.Elapsed.TotalMilliseconds;

            var d = Convert.ToInt32(FPS_DELAY - logicCostMilliseconds);
            if (d <= 1) d = 1;

            await Task.Delay(d);
        }
    }

    public void Stop()
    {
        _isRunning = false;
    }

    private async Task OnLogic(float elapsedTime)
    {
        var e = new GameLoopLogicEventArgs()
        {
            ElapsedTime = elapsedTime,
        };

        await Logic.Invoke(this, e);
    }
}