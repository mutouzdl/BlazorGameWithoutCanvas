using System.Diagnostics;

namespace FightGame
{
    public class World
    {
        public const int FPS = 90;
        public int FPS_DELAY => Convert.ToInt32(1000.0 / FPS);

        private bool _isRunning = false;
        private decimal _timeCounter = 0;

        public event EventHandler<WorldLogicEventArgs> Logic;

        public async void Start()
        {
            if (_isRunning)
            {
                return;
            }

            _isRunning = true;

            Stopwatch stopWatch = new Stopwatch();
            float deltaTime = 0;
            while (_isRunning || _timeCounter > 0)
            {
                _timeCounter = _isRunning ? 10 : _timeCounter - 1;

                stopWatch.Reset();
                stopWatch.Start();

                this.OnLogic(deltaTime);

                stopWatch.Stop();

                var ms = stopWatch.Elapsed.TotalMilliseconds;

                var d = Convert.ToInt32(FPS_DELAY - ms);
                if (d <= 1) d = 1;

                deltaTime = d / 1000f;

                await Task.Delay(d);
                CurrentFPS = Convert.ToInt32(1000.0 / d);
            }
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public int CurrentFPS { get; private set; } = 0;

        private void OnLogic(float deltaTime)
        {
            var e = new WorldLogicEventArgs()
            {
                DeltaTime = deltaTime,
            };
            Logic?.Invoke(this, e);
        }
    }
}
