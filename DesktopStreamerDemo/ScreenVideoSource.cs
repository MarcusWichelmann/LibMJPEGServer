using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibMJPEGServer;
using LibMJPEGServer.Sources;

namespace DesktopStreamerDemo
{
    public class ScreenVideoSource : VideoSource
    {
        public Screen Screen { get; }
        public int ScreenId { get; }

        public bool IsCapturing => _updateTimer.Enabled;

        private System.Timers.Timer _updateTimer = new System.Timers.Timer(1000 / 25.0);

        private Bitmap _previewImage = null;
        private bool _updatePreview = true;

        public override event EventHandler<FrameCapturedEventArgs> FrameCaptured;

        public ScreenVideoSource(Screen screen, int screenId)
        {
            Screen = screen;
            ScreenId = screenId;

            _updateTimer.Elapsed += _updateTimer_Elapsed;
        }

        public override void StartCapture() => _updateTimer.Start();
        public override void StopCapture() => _updateTimer.Stop();

        public Image QueryPreviewImage()
        {
            _updatePreview = true;

            if(_previewImage == null)
                return null;

            Image previewImage;

            lock(_previewImage)
                previewImage = (Image)_previewImage.Clone();

            return previewImage;
        }

        private void _updateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Bitmap screenshot = new Bitmap(Screen.Bounds.Width, Screen.Bounds.Height, PixelFormat.Format32bppArgb);

            using(Graphics graphics = Graphics.FromImage(screenshot))
                graphics.CopyFromScreen(Screen.Bounds.X, Screen.Bounds.Y, 0, 0, Screen.Bounds.Size, CopyPixelOperation.SourceCopy);

            if(_updatePreview)
            {
                if(_previewImage == null)
                {
                    _previewImage = new Bitmap(screenshot, new Size(160, 160));
                }
                else
                {
                    lock(_previewImage)
                    {
                        _previewImage?.Dispose();
                        _previewImage = new Bitmap(screenshot, new Size(160, 160));
                    }
                }

                _updatePreview = false;
            }

            FrameCaptured?.Invoke(this, new FrameCapturedEventArgs(screenshot));
        }
    }
}
