using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;

namespace LibMJPEGServer.Sources
{
    public class TestVideoSource : VideoSource
    {
        private Timer _updateTimer = new Timer(1000 / 30.0);

        private Random _random = new Random();

        private Image _testFrame = new Bitmap(1280, 720);
        private Graphics _graphics;

        public event EventHandler<FrameCapturedEventArgs> FrameCaptured;

        public TestVideoSource()
        {
            _graphics = Graphics.FromImage(_testFrame);

            for(int x = 0; x < _testFrame.Width; x += _random.Next(0, _testFrame.Width / 30))
                for(int y = 0; y < _testFrame.Height; y += _random.Next(0, _testFrame.Height / 20))
                    _graphics.FillRectangle(GetRandomBrush(), x, y, _testFrame.Width - x, _testFrame.Height - y);

            _updateTimer.Elapsed += OnElapsed;
        }

        public void StartCapture() => _updateTimer.Start();
        public void StopCapture() => _updateTimer.Stop();

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            _graphics.FillRectangle(GetRandomBrush(), _random.Next(0, _testFrame.Width), _random.Next(0, _testFrame.Height), _random.Next(20, 70), _random.Next(20, 70));
            FrameCaptured?.Invoke(this, new FrameCapturedEventArgs((Image)_testFrame.Clone()));
        }

        private Brush GetRandomBrush()
        {
            Color color = Color.FromArgb(_random.Next(0, 256), _random.Next(0, 256), _random.Next(0, 256));
            return new SolidBrush(color);
        }
    }
}