using System.Diagnostics;

namespace HomeOffice.Controllers
{
    public class TimeService : ITimeService
    {
        private readonly Stopwatch _stopwatch;

        public TimeService()
        {
            _stopwatch = new Stopwatch();
        }

        public void Start()
        {
            _stopwatch.Start();
        }

        public void Stop()
        {
            _stopwatch.Stop();
        }

        public int GetElapsedMinutes()
        {
            return _stopwatch.Elapsed.Minutes;
        }

        
    }
}
