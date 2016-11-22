using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LibMJPEGServer
{
    public class FpsCounter
    {
        public int UpdateInterval { get; }
        public int CurrentFps { get; private set; } = -1;

        private Timer _timer;
        private object _lock = new object();

        private int _tickCount = 0;

        public FpsCounter(int updateInterval)
        {
            UpdateInterval = updateInterval;

            _timer = new Timer(1000 * UpdateInterval);
            _timer.Elapsed += OnElapsed;
            _timer.Start();
        }

        public void Tick()
        {
            lock(_lock)
                _tickCount++;
        }

        private void OnElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock(_lock)
            {
                CurrentFps = (int)Math.Round((double)_tickCount / UpdateInterval);
                _tickCount = 0;
            }
        }
    }
}
